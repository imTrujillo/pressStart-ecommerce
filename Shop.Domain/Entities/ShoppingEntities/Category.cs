namespace Shop.Domain.Entities.ShoppingEntities;

public class Category :  BaseEntity
{
    public Category()
    {
        Products = new List<Product>();
    }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}