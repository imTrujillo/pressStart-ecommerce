using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.PDFs;

namespace Shop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderInvoice(int orderId)
        {
            var invoice = await _invoiceService.GetInvoiceByOrderId(orderId);
            
            var invoiceDocument = new InvoiceDocument(invoice);
            var pdfBytes = invoiceDocument.GeneratePdf();
            var filename = $"Factura_{invoice.InvoiceNumber}.pdf";

            return File(pdfBytes, "application/pdf", filename);
        }
    }
}
