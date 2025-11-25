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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<CreateOrderDto> _validator;

        public OrdersController(IOrderService orderService, IValidator<CreateOrderDto> validator)
        {
            _orderService = orderService;
            _validator = validator;
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId);
            return Ok(order);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            var createdOrder = await _orderService.CreateCompleteOrderAsync(dto);
            return CreatedAtAction(nameof(GetOrder), new { createdOrder.OrderId }, createdOrder);
        }

        [HttpPut("update/{orderId:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder([FromBody]CreateOrderDto dto, int orderId)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = TypedResults.ValidationProblem(validationResult.ToDictionary());
                return BadRequest(errors);
            }
            
            var updatedOrder = await _orderService.UpdateOrderAsync(dto, orderId);
            return Ok(updatedOrder);
        }

        [HttpDelete("delete/{orderId:int}")]

        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOrderAsync(orderId);
            return NoContent();
        }

        [HttpPost("{orderId:int}/product/{productId:int}")]
        public async Task<IActionResult> AddProductToOrder(int orderId, int productId, int quantity)
        {
            await _orderService.AddProductToOrder(orderId, productId, quantity);
            return Ok("Product Added Successfully");
        }

        [HttpDelete("{orderId:int}/product/{productId:int}")]
        public async Task<IActionResult> DeleteProductFromOrder(int orderId, int productId)
        {
            await _orderService.DeleteProductFromOrderAsync(orderId, productId);
            return Ok("Product deleted successfully");
        }

        [HttpPut("{orderId:int}/product/{productId:int}")]
        public async Task<IActionResult> UpdateProductQuantity(int orderId, int productId, int quantity)
        {
            await _orderService.UpdateProductQuantityInOrder(orderId, productId, quantity);
            return Ok("Product Quantity updated successfully");
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetOrdersByUserAsync(int userId)
        {
            var orders = await _orderService.GetOrdersByClient(userId);
            return Ok(orders);
        }
    }
}
