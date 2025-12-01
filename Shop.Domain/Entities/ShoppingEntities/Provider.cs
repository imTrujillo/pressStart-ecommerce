namespace Shop.Domain.Entities.ShoppingEntities;

public class Provider : BaseEntity
{
    public Provider()
    {
        Products = new List<Product>();
    }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}