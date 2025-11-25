using System.Security.Claims;
using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Services;

public interface ITokenService
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    DateTime GetTokenExpiration();
}