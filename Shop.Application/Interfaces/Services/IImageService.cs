using Microsoft.AspNetCore.Http;
using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Services;

public interface IImageService
{
    Task<ImageEntity> SaveImageAsync(IFormFile file, int productId, bool isMain = false);
    Task<bool> DeleteImageAsync(string imagePath);
    bool ValidateImage(IFormFile file);
    string GenerateUniqueName(string originalName, int productId);
    string GetPublicUrl(string imagePath);
}