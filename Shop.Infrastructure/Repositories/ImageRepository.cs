using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Data;

namespace Shop.Infrastructure.Repositories;

public class ImageRepository : BaseRepository<ImageEntity>, IImageRepository
{
    public ImageRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ImageEntity>> GetAllByProductIdAsync(int productId)
    {
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        
        return await Context.Images.Where(i => i.ProductId ==  productId)
            .OrderBy(i => i.Order)
            .ThenByDescending(i => i.IsMainImage)
            .ThenBy(i => i.CreatedAt).ToListAsync();
    }

    public async Task<ImageEntity> GetMainProductImageAsync(int productId)
    {
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        
        var mainImage = await Context.Images.Where(i => i.ProductId == productId && i.IsMainImage)
            .FirstOrDefaultAsync();

        if (mainImage is null)
            throw new CustomNotFoundException($"The Main Image with productId {productId} doesn't exist");
        
        return mainImage;
    }

    public async Task<bool> SetImageAsMainAsync(int imageId)
    {
        if (imageId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(imageId));
        
        var newMainImage = await Context.Images.Where(i => i.Id == imageId).FirstOrDefaultAsync();
        
        if (newMainImage is null)
            throw new CustomNotFoundException("The Image doesn't exist");
        
        var productImages = await Context.Images.Where(i => i.ProductId == newMainImage.ProductId)
            .ToListAsync();
        
        foreach (var image in productImages)
        {
            image.IsMainImage = false;
        }

        newMainImage.IsMainImage = true;
        await Context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<bool> DeleteImagesAsync(int productId)
    {
        if (productId <= 0) throw new ArgumentException("Id must be greater than 0", nameof(productId));
        
        var images = await Context.Images.Where(i => i.ProductId == productId).ToListAsync();

        if (images.Any())
        {
            Context.Images.RemoveRange(images);
            await Context.SaveChangesAsync();
        }

        return true;
    }
}