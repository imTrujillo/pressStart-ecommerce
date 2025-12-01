namespace Shop.Application.DTOs.Request.Auth;

public record ResetPassword
{
    public string Username { get; init; }
    public string Dui { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmPassword { get; init; }
}