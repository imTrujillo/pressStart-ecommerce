namespace Shop.Application.DTOs.Request.Shopping;

public record CreateProviderDto
{
    public string Name { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public bool IsActive { get; set; }
}