using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Address>> GetByUserIdAsync(int userId)
    {
        return await Context.Addresses.Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.AddressName).ToListAsync();
    }

    public override async Task<Address> GetByIdAsync(int id)
    {
        var address = await Context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        
        if (address is null) throw new CustomNotFoundException($"Address with the Id {id} was not Found");

        return address;
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var address = await GetByIdAsync(id);
        
        address.IsActive = false;
        address.UpdateAt = DateTime.UtcNow;
        await Context.SaveChangesAsync();
        
        return true;
    }
}