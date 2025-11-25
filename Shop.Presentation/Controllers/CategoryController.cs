using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CreateCategoryDto> _validator;

        public CategoryController(ICategoryService categoryService, IValidator<CreateCategoryDto> validator)
        {
            _categoryService = categoryService;
            _validator = validator;
        }
        
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCategory(CreateCategoryDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            return Ok(await _categoryService.CreateCategoryAsync(dto));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            return Ok(await _categoryService.GetByIdAsync(id));
        }

        [HttpDelete]
        [Route("category/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            return Ok(await _categoryService.DeleteCategoryAsync(id));
        }

        [HttpPut]
        [Route("category/update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(CreateCategoryDto dto, int id)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            return Ok(await _categoryService.UpdateCategoryAsync(dto, id));
        }
    }
}
