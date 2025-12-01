using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<Payment> GetByStripeSessionIdAsync(string stripeSessionId)
    {
        var payment = await Context.Payments.FirstOrDefaultAsync(p => p.StripeSessionId == stripeSessionId);

        if (payment is null)
            throw new CustomNotFoundException($"Payment with StripeSessionId {stripeSessionId} does not exist");

        return payment;
    }
}