using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ShopDbContext context) : base(context)
    {
    }
}