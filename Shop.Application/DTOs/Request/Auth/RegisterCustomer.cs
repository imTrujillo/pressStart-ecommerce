namespace Shop.Application.DTOs.Request;

public record RegisterCustomer
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string FullName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Dui { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
}