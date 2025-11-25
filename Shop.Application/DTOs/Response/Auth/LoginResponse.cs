namespace Shop.Application.DTOs.Response.Auth;

public record LoginResponse
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiresAt { get; init; }
    public UserResponseDto User { get; init; }
}