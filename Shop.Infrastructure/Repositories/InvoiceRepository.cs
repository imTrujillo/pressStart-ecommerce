using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Enums;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class InvoiceRepository : BaseRepository<Invoice> , IInvoiceRepository
{
    public InvoiceRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<Invoice> GetInvoiceByOrder(int orderId)
    {
        if (orderId <= 0) throw new ArgumentException("Order Id must be greater than 0", nameof(orderId));
        
        var invoice = await Context.Invoices
            .Include(i => i.Order)
                .ThenInclude(o => o.Details)
                    .ThenInclude(d => d.Product)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.OrderId == orderId && i.Order.Status == OrderStatus.Paid);

        if (invoice is null)
            throw new CustomNotFoundException($"Invoice with orderId {orderId} was not found");

        return invoice;
    }
    
}