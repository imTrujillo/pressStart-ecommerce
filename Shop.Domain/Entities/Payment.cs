using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Enums;

namespace Shop.Domain.Entities;

public class Payment : BaseEntity
{
    public int OrderId { get; set; }
    public string StripeSessionId { get; set; } = String.Empty;
    public string? StripeIntentId { get; set; } = String.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "usd";
    public PaymentStatus Status { get; set; }
    public DateTime? PaymentDate { get; set; }
    public virtual Order Order { get; set; }
}