namespace Shop.Application.DTOs.Response.Auth;

public record UserResponseDto
{
    public int Id { get; init; }
    public string Username { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
    public DateTime LastLogin { get; init; }
}