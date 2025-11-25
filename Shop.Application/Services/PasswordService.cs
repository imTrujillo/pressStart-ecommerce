using Shop.Application.Interfaces.Services;

namespace Shop.Application.Services;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, 12);
    }

    public bool VerifyHashedPassword(string passwordHash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}