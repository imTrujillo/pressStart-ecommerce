using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IInvoiceRepository : IBaseRepository<Invoice>
{
    Task<Invoice> GetInvoiceByOrder(int orderId);
}