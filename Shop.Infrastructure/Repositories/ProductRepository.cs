using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product> , IProductRepository
{
    public ProductRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        if (categoryId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(categoryId));
        
        return await Context.Products.AsNoTracking()
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Include(p => p.Provider)
            .AsSplitQuery().ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByProviderAsync(int providerId)
    {
        if (providerId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(providerId));
        
        return await Context.Products.AsNoTracking()
            .Where(p => p.ProviderId == providerId)
            .Include(p => p.Provider)
            .Include(p => p.Category)
            .AsSplitQuery().ToListAsync();
    }

    public async Task<Product> GetCompleteProductAsync(int productId)
    {
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        
        return await Context.Products.AsNoTracking()
            .Include(p => p.Provider)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.Details)
                .ThenInclude(d => d.Order)
                    .ThenInclude(o => o.User)
            .AsSplitQuery().FirstOrDefaultAsync(p => p.Id == productId); 
    }
}