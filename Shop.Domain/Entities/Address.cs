namespace Shop.Domain.Entities;

public class Address : BaseEntity
{
    public int UserId { get; set; }
    public string? AddressName { get; set; }
    
    public string? AddressLine { get; set; } 
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public bool IsDefault { get; set; }
    
    public User User { get; set; }
}