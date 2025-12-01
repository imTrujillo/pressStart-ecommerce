using Shop.Domain.Enums;

namespace Shop.Domain.Entities.ShoppingEntities;

public class Order : BaseEntity
{
    public Order()
    {
        Details = new List<OrderDetail>();
        Payments = new List<Payment>();
    }
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }
    public string Address { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public virtual Invoice Invoice { get; set; }
    public virtual List<OrderDetail> Details { get; set; }

    public ICollection<Payment> Payments { get; set; }
}