using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Enums;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetByClientAsync(int clientId)
    {
        if (clientId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(clientId));
        
        return await Context.Orders.AsNoTracking()
            .Where(o => o.UserId == clientId)
            .Include(o => o.User)
            .Include(o => o.Details)
            .ThenInclude(dp => dp.Product).AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GeyByStatusAsync(OrderStatus status)
    {
        return await Context.Orders.AsNoTracking()
            .Where(o => o.Status == status)
            .Include(o => o.User)
            .Include(o => o.Details).AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Order> GetCompleteOrderAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("Id must be greater than 0", nameof(id));
        
        return await Context.Orders
            .Include(o => o.User)
            .Include(o => o.Details)
            .ThenInclude(d => d.Product)
            .ThenInclude(p => p.Category)
            .Include(o => o.Details)
            .ThenInclude(d => d.Product)
            .ThenInclude(p => p.Category).AsSplitQuery().FirstOrDefaultAsync(o => o.Id == id);
    }
}