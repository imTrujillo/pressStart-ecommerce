using Microsoft.AspNetCore.Http;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.DTOs.Response.Shopping;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Exceptions;

namespace Shop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;

    public ProductService(IProductRepository productRepository
        , IImageRepository imageRepository, IImageService imageService)
    {
        _productRepository = productRepository;
        _imageRepository = imageRepository;
        _imageService = imageService;
    }
    
    public async Task<ProductWithImageDto> CreateProductWithImagesAsync(CreateProductDto dto, List<IFormFile> images)
    {
        var newProduct = new Product()
        {
            Name = dto.ProductName,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            ProviderId = dto.ProviderId
        };

        var createdProduct = await _productRepository.AddAsync(newProduct);
        
        if (images.Any())
        {
            for (int i = 0; i < images.Count; i++)
            {
                var file = images[i];
                var esPrincipal = i == 0; // La primera imagen es principal por defecto
                
                // Guardar archivo físico y crear entidad
                var imagenInfo = await _imageService.SaveImageAsync(file, createdProduct.Id, esPrincipal);
                imagenInfo.Order = i + 1;
                
                // Guardar en base de datos
                await _imageRepository.AddAsync(imagenInfo);
            }
        }

        return await GetCompleteProductAsync(createdProduct.Id);
    }

    public async Task<ProductWithImageDto> GetCompleteProductAsync(int productId)
    {
        var product = await _productRepository.GetCompleteProductAsync(productId);

        var images = await _imageRepository.GetAllByProductIdAsync(productId);

        return new ProductWithImageDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryName = product.Category?.Name ?? "Sin Categoría",
            ProviderName = product.Provider?.Name ?? "Sin Proveedor",
            Images = images.Select(i => new ImageDto()
            {
                ImageId = i.Id,
                FileName = i.FileName,
                ImageUrl = i.ImageUrl,
                IsMain = i.IsMainImage,
                Order = i.Order
            }).ToList()
        };
    }

    public async Task<ImageDto> AddImageToProduct(int productId, IFormFile file)
    {
        var exists = await _productRepository.ExistAsync(productId);

        if (!exists)
            throw new CustomNotFoundException("The Product Does Not Exists");

        var existingImages = await _imageRepository.GetAllByProductIdAsync(productId);
        var newOrder = existingImages.Any() ? existingImages.Max(i => i.Order) + 1 : 1;
        
        var isMain = !existingImages.Any();

        var imageInfo = await _imageService.SaveImageAsync(file, productId, isMain);
        imageInfo.Order = newOrder;

        var savedImage = await _imageRepository.AddAsync(imageInfo);

        return new ImageDto()
        {
            ImageId = savedImage.Id,
            FileName = savedImage.FileName,
            ImageUrl = savedImage.ImageUrl,
            IsMain = savedImage.IsMainImage,
            Order = savedImage.Order
        };
    }

    public async Task<bool> DeleteImageAsync(int imageId)
    {
        var image = await _imageRepository.GetByIdAsync(imageId);

        await _imageService.DeleteImageAsync(image.ImagePath);
        await _imageRepository.DeleteAsync(imageId);

        return true;
    }
} 