using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Enums;

namespace Shop.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        Addresses =  new List<Address>();
        Orders = new List<Order>();
        Invoices = new List<Invoice>();
    }
    
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Dui { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string RefreshToken  { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; }
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime LastLogin { get; set; }
    //Employees Only
    public DateTime? HireDate { get; set; }
    public decimal? Salary  { get; set; }
    public string? Nit { get; set; }
    
    //One-Many Relationships with Address
    public virtual ICollection<Address> Addresses { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Invoice> Invoices { get; set; }
}