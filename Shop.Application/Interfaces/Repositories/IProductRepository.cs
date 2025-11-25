using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetByProviderAsync(int providerId);
    
    Task<Product> GetCompleteProductAsync(int productId);
}