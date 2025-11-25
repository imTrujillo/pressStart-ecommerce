using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.Interfaces.Services;


namespace Shop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly IValidator<CreateProviderDto> _validator;

        public ProviderController(IProviderService providerService, IValidator<CreateProviderDto> validator)
        {
            _providerService = providerService;
            _validator = validator;
        }
        
        [HttpGet]
        public IActionResult GetProviders()
        {
            return Ok(_providerService.GetAllProviders());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProvider(CreateProviderDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            return Ok(await _providerService.CreateProviderAsync(dto));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProviderById(int id)
        {
            return Ok(await _providerService.GetByIdAsync(id));
        }

        [HttpDelete]
        [Route("provider/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            return Ok(await _providerService.DeleteProviderAsync(id));
        }

        [HttpPut]
        [Route("provider/update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProvider(CreateProviderDto dto, int id)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            return Ok(await _providerService.UpdateProviderAsync(dto, id));
        }
    }
}
