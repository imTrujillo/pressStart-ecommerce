using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.Request.Shopping;
using Shop.Application.DTOs.Response.Shopping;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IDetailsRepository _detailsRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductDto> _validator;

        public ProductController(
            IProductRepository productRepository, 
            IDetailsRepository detailsRepository,
            IImageRepository imageRepository,
            IProductService productService,
            IValidator<CreateProductDto> validator)
        {
            _productRepository = productRepository;
            _detailsRepository = detailsRepository;
            _imageRepository = imageRepository;
            _productService = productService;
            _validator = validator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProductAsync([FromForm] CreateProductDto dto, [FromForm]List<IFormFile> images)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            var createdProduct = await _productService.CreateProductWithImagesAsync(dto, images);
            return CreatedAtAction(nameof(GetProductById), new { Id = createdProduct.ProductId }, createdProduct);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetCompleteProductAsync(id);

            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]CreateProductDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            var existingProduct = await _productRepository.GetByIdAsync(id);

            existingProduct.Name = dto.ProductName;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;
            existingProduct.CategoryId = dto.CategoryId;
            existingProduct.ProviderId = dto.ProviderId;
            
            await _productRepository.UpdateAsync(existingProduct);

            var updatedProduct = await _productRepository.GetCompleteProductAsync(id);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var orders = await _detailsRepository.GetOrdersWithProducts(id);

            if (orders.Any())
                return Conflict("Product can't be deleted because it has orders");

            var images = await _imageRepository.GetAllByProductIdAsync(id);

            foreach (var image in images)
            {
                await _productService.DeleteImageAsync(image.Id);
            }

            await _productRepository.DeleteAsync(id);

            return NoContent();
        }
        
        //Images Operations

        [HttpPost]
        [Route("{productId}/Images")]
        [Authorize]
        public async Task<IActionResult> AddImage(int productId, [FromForm]ImageRequest request)
        {
            if (request.Image.Length == 0) return BadRequest("Add a Valid Image");

            var createdImage = await _productService.AddImageToProduct(productId, request.Image);

            return CreatedAtAction(nameof(GetImage), new { productId = productId, imageId = createdImage.ImageId }, createdImage);
        }

        [HttpPost]
        [Route("{productId}/Images/Multiple")]
        [Authorize]
        public async Task<IActionResult> AddMultipleImages(int productId, [FromForm] List<IFormFile> images)
        {
            if (!images.Any())
                return BadRequest("You must add at least one images");

            var createdImages = new List<ImageDto>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var createdImage = await _productService.AddImageToProduct(productId, image);
                    createdImages.Add(createdImage);
                }
            }

            return Ok(createdImages);
        }

        [HttpGet]
        [Route("{productId}/Images/{imageId}")]
        public async Task<IActionResult> GetImage(int productId, int imageId)
        {
            var image = await _imageRepository.GetByIdAsync(imageId);
            
            if (image.ProductId != productId) return NotFound("That image does not exist");

            var imageDto = new ImageDto()
            {
                ImageId = image.Id,
                FileName = image.FileName,
                ImageUrl = image.ImageUrl,
                IsMain = image.IsMainImage,
                Order = image.Order
            };

            return Ok(imageDto);
        }

        [HttpGet]
        [Route("{productId:int}/Images")]
        public async Task<IActionResult> GetAllProductImages(int productId)
        {
            var images = await _imageRepository.GetAllByProductIdAsync(productId);

            var imagesDto = images.Select(image => new ImageDto()
            {
                ImageId = image.Id,
                FileName = image.FileName,
                ImageUrl = image.ImageUrl,
                IsMain = image.IsMainImage,
                Order = image.Order
            }).ToList();

            return Ok(imagesDto);
        }

        [HttpDelete]
        [Route("{productId}/Images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProductImage(int productId, int imageId)
        {
            var image = await _imageRepository.GetByIdAsync(imageId);

            if (image.ProductId != productId)
                return BadRequest("Image not Found");
            
            await _productService.DeleteImageAsync(imageId);
            return NoContent();
        }

        [HttpPut]
        [Route("{productId}/Images/{imageId}/Principal")]
        [Authorize]
        public async Task<IActionResult> SetMainProductImage(int productId, int imageId)
        {
            var image = await _imageRepository.GetByIdAsync(imageId);
            
            if (image.ProductId != productId)
                return BadRequest("Image not Found");

            await _imageRepository.SetImageAsMainAsync(imageId);

            return Ok(image);
        }

        [HttpGet]
        [Route("{productId}/Orders")]
        public async Task<IActionResult> GetOrdersWithProduct(int productId)
        {
            var ordersWithProduct = await _detailsRepository.GetOrdersWithProducts(productId);

            var result = ordersWithProduct.Select(d => new
            {
                OrderId = d.OrderId,
                User = d.Order.User.Username,
                Date = d.Order.Date,
                Status = d.Order.Status,
                Quantity = d.Quantity,
                Price = d.Price,
                SubTotal = d.Quantity * d.Price
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? providerId = null,
            [FromQuery] string? search = "")
        {
            IEnumerable<Product> products;
            
            if (categoryId.HasValue)
            {
                products = await _productRepository.GetByCategoryAsync(categoryId.Value);
            }
            else if (providerId.HasValue)
            {
                products = await _productRepository.GetByProviderAsync(providerId.Value);
            }
            else
            {
                products = await _productRepository.GetAllAsync();
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => 
                    p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            
            var paginatedProducts = products.Skip((page - 1) * size).Take(size);

            var productsDto = new List<ProductWithImageDto>();

            foreach (var product in products)
            {
                var completeProduct = await _productService.GetCompleteProductAsync(product.Id);

                if (completeProduct is not null)
                {
                    productsDto.Add(completeProduct);
                }
            }

            return Ok(new
            {
                Products = productsDto,
                TotalProducts = products.Count(),
                Page = page,
                PageSize = size,
                TotalPages = (int)Math.Ceiling((double)products.Count() / size)
            });
        }
        
    }
}
