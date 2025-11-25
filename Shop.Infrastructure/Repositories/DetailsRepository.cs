using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class DetailsRepository : BaseRepository<OrderDetail>, IDetailsRepository
{
    public DetailsRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OrderDetail>> GetDetailsByOrderAsync(int orderId)
    {
        if (orderId <= 0) throw new ArgumentException("Order Id must be greater than 0", nameof(orderId));
        
        return await Context.OrderDetails.AsNoTracking()
            .Where(d => d.OrderId == orderId)
            .Include(d => d.Product)
            .ThenInclude(p => p.Category)
            .ToListAsync();
    }

    public async Task<OrderDetail> GetDetail(int orderId, int productId)
    {
        if (orderId <= 0) throw new ArgumentException("Order Id must be greater than 0", nameof(orderId));
        if (productId <= 0) throw new ArgumentException("Product Id must be greater than 0", nameof(productId));
        
        var detail = await Context.OrderDetails.FirstOrDefaultAsync(d
            => d.ProductId == productId && d.OrderId == orderId);

        if (detail is null) throw new CustomNotFoundException("Pedido No Encontrado");

        return detail;
    }

    public async Task<OrderDetail> AddProductToDetail(int orderId, int productId, int quantity, float price)
    {
        if (orderId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(orderId));
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0");
        if (price <= 0.0f) throw new ArgumentException("Price must be greater than 0");
        
        var orderDetail = new OrderDetail()
        {
            Quantity = quantity,
            Price = price,
            OrderId = orderId,
            ProductId = productId
        };
        
        Context.OrderDetails.Add(orderDetail);
        await Context.SaveChangesAsync();

        return await GetDetail(orderDetail.OrderId, productId);
    }

    public async Task<OrderDetail> UpdateOrderDetail(int orderId, int productId, int quantity, float price)
    {
        if (orderId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(orderId));
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0");
        if (price <= 0.0f) throw new ArgumentException("Price must be greater than 0");
        
        var existingDetail = await GetDetail(orderId, productId);
        
        existingDetail.Quantity = quantity;
        existingDetail.Price = price;
        
        var updatedDetail = await UpdateAsync(existingDetail);

        return updatedDetail;
    }

    public async Task<bool> DeleteProductFromDetail(int orderId, int productId)
    {
        if (orderId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(orderId));
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        
        var existingDetail = await GetDetail(orderId, productId);

        Context.OrderDetails.Remove(existingDetail);
        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAllDetailsAsync(int orderId)
    {
        if (orderId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(orderId));
        
        var details = await Context.OrderDetails
            .Where(d => d.OrderId == orderId).ToListAsync();

        if (details.Any())
        {
            //Remover un Rango de Elementos con RemoveRange
            Context.OrderDetails.RemoveRange(details);
            await Context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<float> CalculateOrderTotal(int orderId)
    {
        if (orderId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(orderId));
        
        return await Context.OrderDetails.Where(d => d.OrderId == orderId)
            .SumAsync(d => d.Quantity * d.Price);
    }

    public async Task<IEnumerable<OrderDetail>> GetOrdersWithProducts(int productId)
    {
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        
        return await Context.OrderDetails
            .Where(d => d.ProductId == productId)
            .Include(d => d.Order)
                .ThenInclude(o => o.User)
            .AsSplitQuery().ToListAsync();
    }
}