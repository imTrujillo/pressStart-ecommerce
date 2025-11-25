namespace Shop.Application.DTOs.Request.Auth;

public class RegisterEmployee
{
    public string Username { get; init; }
    public string Password { get; init; }
    public string FullName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Dui { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public DateTime HireDate { get; init; }
    public decimal Salary { get; init; }
    public string Nit { get; init; }
}