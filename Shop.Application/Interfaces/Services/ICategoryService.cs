using Shop.Application.DTOs.Request.Shopping;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryService
{
    List<Category> GetAllCategories();
    Task<Category> GetByIdAsync(int id);
    Task<Category> CreateCategoryAsync(CreateCategoryDto dto);
    Task<Category> UpdateCategoryAsync(CreateCategoryDto dto, int categoryId);
    Task<bool> DeleteCategoryAsync(int categoryId);
}