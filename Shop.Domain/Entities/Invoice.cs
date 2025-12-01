using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Domain.Entities;

public class Invoice : BaseEntity
{
    // public string CompanyName = "PressStart";
    // public string LogoUrl = "/home/assisted/RiderProjects/Shop.Presentation/Shop.Presentation/wwwroot/uploads/logo.png";
    public Guid InvoiceNumber { get; set; }
    public string UserDui { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public string UserEmail { get; set; } = String.Empty;
    public string UserPhone { get; set; } = String.Empty;
    public string UserAddress { get; set; } = String.Empty;
    
    public DateTime IssueDate { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public decimal Total { get; set; }
}