using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.DTOs.Response.Shopping;

namespace Shop.Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderResponseDto> CreateCompleteOrderAsync(CreateOrderDto dto);
    Task<OrderResponseDto> UpdateOrderAsync(CreateOrderDto dto, int orderId);
    Task<bool> DeleteOrderAsync(int orderId);
    Task AddProductToOrder(int orderId, int productId, int quantity);
    Task DeleteProductFromOrderAsync(int orderId, int productId);
    Task UpdateProductQuantityInOrder(int orderId, int productId, int newQuantity);
    Task<OrderResponseDto> GetOrderAsync(int orderId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByClient(int userId);
}