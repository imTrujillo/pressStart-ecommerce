using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface IImageRepository : IBaseRepository<ImageEntity>
{
    Task<IEnumerable<ImageEntity>> GetAllByProductIdAsync(int productId);
    Task<ImageEntity> GetMainProductImageAsync(int productId);
    Task<bool> SetImageAsMainAsync(int imageId);
    Task<bool> DeleteImagesAsync(int productId);
}