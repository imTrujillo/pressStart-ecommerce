using System.Reflection.Metadata.Ecma335;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.DTOs.Response.Shopping;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDetailsRepository _detailsRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IDetailsRepository detailsRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _detailsRepository = detailsRepository;
    }

    public async Task<OrderResponseDto> CreateCompleteOrderAsync(CreateOrderDto dto)
    {
        var customerExists = await _userRepository.ExistAsync(dto.UserId);
        if (!customerExists)
            throw new ArgumentException("The Customer does not exist");
        
        if (!dto.Details.Any())
            throw new ArgumentException("The order must have at least one product");

        foreach (var detail in dto.Details)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);

            if (product.Stock < detail.Quantity)
                throw new InvalidOperationException(
                    $"There's not enough for the Product {product.Name}. Current Stock: {product.Stock}");

            if (detail.Quantity < 0)
                throw new ArgumentException("The Quantity must be greater than 0");
        }
        
        var order = new Order()
        {
            UserId = dto.UserId,
            User = await _userRepository.GetByIdAsync(dto.UserId),
            Date = DateTime.UtcNow,
            Status = dto.OrderStatus,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
            
        var createdOrder = await _orderRepository.AddAsync(order);

        var orderDetails = new List<OrderDetail>();

        foreach (var detail in dto.Details)
        {
            var createdDetail =  await _detailsRepository.AddProductToDetail(
                createdOrder.Id, detail.ProductId, detail.Quantity, detail.Price);
            
            orderDetails.Add(createdDetail);
        }

        await UpdateProductsStock(dto.Details, true);
        
        return await MapToDto(createdOrder.Id);
    }

    public async Task<OrderResponseDto> UpdateOrderAsync(CreateOrderDto dto, int orderId)
    {
        var order = await _orderRepository.GetCompleteOrderAsync(orderId);

        var oldDetails = await _detailsRepository.GetDetailsByOrderAsync(orderId);

        var oldDetailsDto = oldDetails.Select(d => new OrderDetailDto()
        {
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            Price = d.Price
        }).ToList();
        
        //Update Products' Stocks
        await UpdateProductsStock(oldDetailsDto, false);
        
        //Delete Old Details
        await _detailsRepository.DeleteAllDetailsAsync(orderId);

        order.Status = dto.OrderStatus;
        order.Address = dto.Address;
        order.UpdateAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(order);

        //Add Product to Details
        foreach (var detail in dto.Details)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);

            if (product.Stock < detail.Quantity) 
                throw new InvalidOperationException($"Insufficient Stock for the Product {product.Name}");

            await _detailsRepository.AddProductToDetail(orderId, detail.ProductId, detail.Quantity, detail.Price);
        }

        await UpdateProductsStock(dto.Details, true);

        return await MapToDto(orderId);
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var details = await _detailsRepository.GetDetailsByOrderAsync(orderId);

        var detailsDto = details.Select(d => new OrderDetailDto()
        {
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            Price = d.Price
        }).ToList();
        
        //Update Product's stock
        await UpdateProductsStock(detailsDto, false);

        //Remove Details
        await _detailsRepository.DeleteAllDetailsAsync(orderId);
        
        //Delete Order
        return await _orderRepository.DeleteAsync(orderId); 
    }

    public async Task AddProductToOrder(int orderId, int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product.Stock < quantity)
            throw new InvalidOperationException("Insufficient Stock");

        await _detailsRepository.AddProductToDetail(
            orderId, productId, quantity, product.Price
        );

        await UpdateProductStock(productId, -quantity);
    }
    
    public async Task DeleteProductFromOrderAsync(int orderId, int productId)
    {
        var detail = await _detailsRepository.GetDetail(orderId, productId);
        if (detail is null)
            throw new ArgumentException("El producto no está en este pedido");
        
        await _detailsRepository.DeleteProductFromDetail(orderId, productId);
        
        await UpdateProductStock(productId, detail.Quantity);
    }
    

    public async Task UpdateProductQuantityInOrder(int orderId, int productId, int newQuantity)
    {
        var currentDetail = await _detailsRepository.GetDetail(orderId, productId);
        
        if (currentDetail is null)
            throw new ArgumentException("El producto no está en este pedido");
        
        var quantityDifference = newQuantity - currentDetail.Quantity;
        
        if (quantityDifference > 0)
        {
            var producto = await _productRepository.GetByIdAsync(productId);
            if (producto.Stock < quantityDifference)
                throw new InvalidOperationException("Stock insuficiente para el aumento solicitado");
        }
        
        await _detailsRepository.UpdateOrderDetail
            (orderId, productId, newQuantity, currentDetail.Price);
        
        await UpdateProductStock(productId, -quantityDifference);
    }

    public async Task<OrderResponseDto> GetOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetCompleteOrderAsync(orderId);
        return await MapToDto(orderId);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByClient(int userId)
    {
        var orders = await _orderRepository.GetByClientAsync(userId);
        
        var ordersDto = new List<OrderResponseDto>();

        foreach (var order in orders)
        {
            var orderDto = await MapToDto(order.Id);
            ordersDto.Add(orderDto);
        }

        return ordersDto;
    }

    //Private Helper Methods
    private async Task<OrderResponseDto> MapToDto(int orderId)
    {
        var order = await _orderRepository.GetCompleteOrderAsync(orderId);
        var total = await _detailsRepository.CalculateOrderTotal(orderId);

        return new OrderResponseDto()
        {
            OrderId  = orderId,
            UserId = order.UserId,
            Date = order.Date,
            Status = order.Status,
            Total = total,
            Details = order.Details.Select(d => new ProductDetailDto()
            {
                ProductName = d.Product.Name,
                Quantity = d.Quantity,
                UnitPrice = d.Price,
                SubTotal = d.Quantity * d.Price
            }).ToList()
        };
    }

    private async Task UpdateProductsStock(ICollection<OrderDetailDto> details, bool subtract)
    {
        foreach (var detail in details)
        {
            var quantity = subtract ? -detail.Quantity : detail.Quantity;
            await UpdateProductStock(detail.ProductId, quantity);
        }
    }

    private async Task UpdateProductStock(int productId, int change)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        
        product.Stock += change;
        
        if (product.Stock < 0)
            throw new InvalidOperationException($"The Stock of the Product {product.Name} can't be negative");

        await _productRepository.UpdateAsync(product);
    }
}