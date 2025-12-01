using Shop.Domain.Entities;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Interfaces.Services;

public interface IInvoiceService
{
    Task<Invoice> CreateInvoice(Order order, Payment payment);
    Task<Invoice> GetInvoiceByOrderId(int orderId);
}