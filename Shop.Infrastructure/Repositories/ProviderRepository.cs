using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class ProviderRepository : BaseRepository<Provider>, IProviderRepository
{
    public ProviderRepository(ShopDbContext context) : base(context)
    {
    }
}