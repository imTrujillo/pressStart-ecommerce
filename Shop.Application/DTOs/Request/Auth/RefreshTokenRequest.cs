namespace Shop.Application.DTOs.Request.Auth;

public record RefreshTokenRequest
{
    //RF Request NEEDS to have both Access and Refresh Token
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
};