using Shop.Domain.Entities;
using Shop.Domain.Enums;

namespace Shop.Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByUsernameAsync(string username);
    Task<bool> CheckUsernameAsync(string username);
    Task<bool> CheckDuiAsync(string dui);
    Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
    Task<User> GeyByRefreshTokenAsync(string refreshToken);
}