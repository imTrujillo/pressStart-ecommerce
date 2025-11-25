using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<IEnumerable<Address>> GetByUserIdAsync(int userId);
    
}