using Shop.Application.DTOs.Request;
using Shop.Application.DTOs.Request.Auth;
using Shop.Application.DTOs.Response.Auth;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities;
using Shop.Domain.Enums;

namespace Shop.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository  _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordService = passwordService;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        
        if (!user.IsActive)
            throw new UnauthorizedAccessException("User is not active");

        if (!_passwordService.VerifyHashedPassword(user.PasswordHash, request.Password))
            throw new UnauthorizedAccessException("Password is incorrect");

        var accessToken = _tokenService.CreateAccessToken(user);
        var refreshToken = _tokenService.CreateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.LastLogin = DateTime.UtcNow;
        
        await _userRepository.UpdateAsync(user);

        return new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = _tokenService.GetTokenExpiration(),
            User = new UserResponseDto()
            {
                Id = user.Id,
                Username =  user.Username,
                FullName = user.Fullname,
                Email = user.Email,
                Role = user.Role.ToString(),
                LastLogin = user.LastLogin
            }
        };
    }

    public async Task<LoginResponse> RefreshTokensAsync(RefreshTokenRequest request)
    {
        //Use AccessToken to get Claims
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var username = principal.Identity?.Name;

        if (string.IsNullOrEmpty(username))
            throw new UnauthorizedAccessException("Token inválido");

        var user = await _userRepository.GetByUsernameAsync(username);

        if (user is null || user.RefreshToken != request.RefreshToken || 
            user.RefreshTokenExpiry <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token inválido o expirado");

        var newAccessToken = _tokenService.CreateAccessToken(user);
        var newRefreshToken = _tokenService.CreateRefreshToken();

        user.RefreshToken = newRefreshToken;
        //Refresh Token Expiry date SHOULD NOT be updated
        //user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            
        await _userRepository.UpdateAsync(user);

        return new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = _tokenService.GetTokenExpiration(),
            User = new UserResponseDto()
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.Fullname,
                Email = user.Email,
                Role = user.Role.ToString(),
                LastLogin = user.LastLogin
            }
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        //var users = await _userRepository.GetByRoleAsync(UserRole.Admin);
        var user = await _userRepository.GeyByRefreshTokenAsync(refreshToken);
        
        user.RefreshToken = String.Empty;
        user.RefreshTokenExpiry = DateTime.MinValue;
        await _userRepository.UpdateAsync(user);
        
        return true;
    }

    public async Task<UserResponseDto> RegisterCustomerAsync(RegisterCustomer request)
    {
        if (await _userRepository.CheckUsernameAsync(request.Username))
            throw new InvalidOperationException("El nombre de usuario ya existe");

        if (await _userRepository.CheckDuiAsync(request.Dui))
            throw new InvalidOperationException("El numero de DUI ya existe");

        if (request.Password != request.ConfirmPassword)
            throw new InvalidOperationException("Las contraseñas no coinciden");
        
        var user = new User
        {
            Username = request.Username,
            PasswordHash = _passwordService.HashPassword(request.Password),
            Fullname = request.FullName,
            DateOfBirth = request.DateOfBirth,
            Dui = request.Dui,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);
        
        return new UserResponseDto()
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            FullName = createdUser.Fullname,
            Email = createdUser.Email,
            Role = createdUser.Role.ToString()
        };
    }

    public async Task<UserResponseDto> RegisterEmployeeAsync(RegisterEmployee request)
    {
        if (await _userRepository.CheckUsernameAsync(request.Username))
            throw new InvalidOperationException("El nombre de usuario ya existe");

        if (await _userRepository.CheckDuiAsync(request.Dui))
            throw new InvalidOperationException("El documento ya está registrado");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = _passwordService.HashPassword(request.Password),
            Fullname = request.FullName,
            DateOfBirth = request.DateOfBirth,
            Dui = request.Dui,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Role = UserRole.Employee,
            HireDate = request.HireDate,
            Salary = request.Salary,
            Nit = request.Nit,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);

        return new UserResponseDto()
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            FullName = createdUser.Fullname,
            Email = createdUser.Email,
            Role = createdUser.Role.ToString()
        };
    }

    public async Task<bool> ResetPasswordAsync(ResetPassword request)
    {
        if (request.NewPassword != request.ConfirmPassword)
            throw new InvalidOperationException("Las contraseñas no coinciden");

        var user = await _userRepository.GetByUsernameAsync(request.Username);
            
        if (user is null || user.Dui != request.Dui)
            throw new UnauthorizedAccessException("Usuario no encontrado o documento inválido");

        user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
        user.RefreshToken = String.Empty; 
        user.RefreshTokenExpiry = DateTime.MinValue;
        user.UpdateAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }
}