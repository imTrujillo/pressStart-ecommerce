using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<Payment> GetByStripeSessionIdAsync(string stripeSessionId);
}