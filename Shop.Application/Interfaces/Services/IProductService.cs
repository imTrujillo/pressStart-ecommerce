using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.DTOs.Response.Shopping;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductWithImageDto> CreateProductWithImagesAsync(CreateProductDto dto, List<IFormFile> images);
    Task<ProductWithImageDto> GetCompleteProductAsync(int productId);
    Task<ImageDto> AddImageToProduct(int productId, IFormFile file);
    Task<bool> DeleteImageAsync(int imageId);
}