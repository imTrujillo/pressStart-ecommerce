using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

//Service to Save Image in a Physical Format
public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly long _maxSize = 5 * 1024 * 1024;
    
    private readonly List<String> AllowedTypes= new List<string>()
    {
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp"
    };

    
    public ImageService(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }
    
    public async Task<ImageEntity> SaveImageAsync(IFormFile file, int productId, bool isMain = false)
    {
        if (!ValidateImage(file))
            throw new ArgumentException("Invalid file format");
    
        var basePath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        
        var uniqueName = GenerateUniqueName(file.FileName, productId);
        var destinedPath = Path.Combine(basePath, "uploads", "productos", DateTime.UtcNow.Year.ToString());

        if (!Directory.Exists(destinedPath))
            Directory.CreateDirectory(destinedPath);
        
        var completePath = Path.Combine(destinedPath, uniqueName);
        var relativePath = Path.Combine("uploads", "productos", DateTime.UtcNow.Year.ToString(), uniqueName);

        using (var stream = new FileStream(completePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var image = new ImageEntity()
        {
            ProductId = productId,
            FileName = uniqueName,
            ImagePath = relativePath.Replace("\\", "/"),
            ImageUrl = GetPublicUrl(relativePath.Replace("\\", "/")),
            BytesSize = file.Length,
            ExtensionType = file.ContentType,
            IsMainImage = isMain,
            CreatedAt = DateTime.UtcNow
        };

        return image;
    }

    public async Task<bool> DeleteImageAsync(string imagePath)
    {
        try
        {
            var basePath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var completePath = Path.Combine(basePath, imagePath.TrimStart('/'));

            if (File.Exists(completePath))
            {
                await Task.Run(() => File.Delete(completePath));
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public bool ValidateImage(IFormFile file)
    {
        if (file.Length == 0) return false;

        if (file.Length > _maxSize) return false;

        if (!AllowedTypes.Contains(file.ContentType.ToLower())) return false;

        var extension = Path.GetExtension(file.FileName).ToLower();
        var allowedExtensions = new List<String>()
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
    
        if (!allowedExtensions.Contains(extension)) return false;

        return true;
    }

    public string GenerateUniqueName(string originalName, int productId)
    {
        var extension = Path.GetExtension(originalName);
        var uniqueName = Guid.NewGuid().ToString()[..8];
        var timeStamp = Stopwatch.GetTimestamp();

        return $"product_{productId}_{timeStamp}_{uniqueName}-{extension}";
    }

    public string GetPublicUrl(string imagePath)
    {
        var baseUrl = _configuration["BaseUrl"] ?? "https://pressstart-api.onrender.com";
        return $"{baseUrl}/{imagePath}";
    }
}