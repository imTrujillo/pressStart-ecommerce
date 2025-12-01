using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Enums;

namespace Shop.Application.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IEnumerable<Order>> GetByClientAsync(int clientId);
    Task<IEnumerable<Order>> GeyByStatusAsync(OrderStatus status);
    Task<Order> GetCompleteOrderAsync(int id);
}