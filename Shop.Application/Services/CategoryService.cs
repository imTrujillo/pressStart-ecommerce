using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public List<Category> GetAllCategories()
    {
        var categories =  _categoryRepository.GetAllQueryable().ToList();
        return categories;
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category;
    }

    public async Task<Category> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category()
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
        
        var createdCategory = await _categoryRepository.AddAsync(category);

        return createdCategory;
    }

    public async Task<Category> UpdateCategoryAsync(CreateCategoryDto dto, int categoryId)
    {
        var currentCategory = await _categoryRepository.GetByIdAsync(categoryId);
        
        currentCategory.Name = dto.Name;
        currentCategory.Description = dto.Description;
        currentCategory.UpdateAt = DateTime.UtcNow;
        currentCategory.IsActive = dto.IsActive;
        
        var updatedCategory = await _categoryRepository.UpdateAsync(currentCategory);
        return updatedCategory;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        return await _categoryRepository.DeleteAsync(categoryId);
    }
}