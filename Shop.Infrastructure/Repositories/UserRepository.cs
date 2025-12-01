using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Enums;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = await Context.Users.Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (user is null) throw new CustomNotFoundException($"User with Username {username} was not Found");

        return user;
    }

    public async Task<bool> CheckUsernameAsync(string username)
    {
        return await Context.Users.AnyAsync(u => u.Username == username &&  u.IsActive);
    }

    public async Task<bool> CheckDuiAsync(string dui)
    {
        return await Context.Users.AnyAsync(u => u.Dui == dui && u.IsActive);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
    {
        return await Context.Users.Where(u => u.Role == role && u.IsActive).ToListAsync();
    }

    public async Task<User> GeyByRefreshTokenAsync(string refreshToken)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        
        if (user is null) throw new CustomNotFoundException($"User was not Found");
        
        return user;
    }

    public override async Task<User> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentNullException("Id must be greater than 0", nameof(id));
        
        var user = await Context.Users.Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null) throw new CustomNotFoundException($"User with Id {id} was not Found");

        return user;
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);

        user.IsActive = false;
        user.UpdateAt = DateTime.UtcNow;
        await Context.SaveChangesAsync();

        return true;
    }
}