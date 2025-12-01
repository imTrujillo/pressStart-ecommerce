using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities;
using Shop.Domain.Entities.ShoppingEntities;
namespace Shop.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository =  invoiceRepository;
    }
    
    public async Task<Invoice> CreateInvoice(Order order, Payment payment)
    {
        var invoice = new Invoice()
        {
            InvoiceNumber = Guid.NewGuid(),
            User =  order.User,
            UserDui = order.User.Dui,
            UserName = order.User.Fullname,
            UserEmail = order.User.Email,
            UserAddress = order.User.Address,
            UserPhone = order.User.PhoneNumber,
            IssueDate = DateTime.UtcNow,
            Order = order,
            OrderId = order.Id,
            Total = payment.Amount,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };

        await _invoiceRepository.AddAsync(invoice);

        return invoice;
    }

    public async Task<Invoice> GetInvoiceByOrderId(int orderId)
    {
        return await _invoiceRepository.GetInvoiceByOrder(orderId);
    }
}