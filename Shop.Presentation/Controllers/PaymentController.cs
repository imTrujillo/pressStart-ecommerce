using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Application.Services;

namespace Shop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout(int orderId)
        {
            var response = await _service.Checkout(orderId);
            return Ok(response);
        }

        [HttpPost]
        [Route("Webhook")]
        public async Task<IActionResult> Webhook()
        {
            var result = await _service.WebHook();
            return Ok(result);
        }
    }
}
