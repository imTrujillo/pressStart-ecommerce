using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;
using Shop.Application.DTOs.Response;
using Shop.Application.DTOs.Response.Auth;

namespace Shop.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshTokensAsync(RefreshTokenRequest request);
    Task<bool> LogoutAsync(string refreshToken);
    Task<UserResponseDto> RegisterCustomerAsync(RegisterCustomer request);
    Task<UserResponseDto> RegisterEmployeeAsync(RegisterEmployee request);
    Task<bool> ResetPasswordAsync(ResetPassword request);
}