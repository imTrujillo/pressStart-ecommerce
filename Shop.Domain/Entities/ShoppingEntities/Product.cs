namespace Shop.Domain.Entities.ShoppingEntities;

public class Product : BaseEntity
{
    public Product()
    {
        Details = new HashSet<OrderDetail>();
        Images = new HashSet<ImageEntity>();
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public int ProviderId { get; set; }
    public virtual Provider? Provider { get; set; }
    public virtual ICollection<OrderDetail> Details { get; set; }
    public virtual ICollection<ImageEntity> Images { get; set; }
}