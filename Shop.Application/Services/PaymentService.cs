using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Interfaces.Services;
using Shop.Domain.Entities;
using Shop.Domain.Enums;
using Shop.Domain.Exceptions;
using Stripe;
using Stripe.Checkout;
using static System.Net.WebRequestMethods;
using StripeEntity = Shop.Domain.Entities.StripeEntity;

namespace Shop.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceService _invoiceService;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly StripeEntity _stripeEntity;

    public PaymentService(
        IConfiguration configuration, 
        IOrderRepository orderRepository, 
        IOptions<StripeEntity> stripeEntity, 
        IPaymentRepository paymentRepository,
        IHttpContextAccessor contextAccessor,
        IInvoiceService invoiceService)
    {
        _orderRepository = orderRepository;
        _stripeEntity = stripeEntity.Value;
        _paymentRepository = paymentRepository;
        _configuration = configuration;
        _contextAccessor = contextAccessor;
        _invoiceService = invoiceService;

        StripeConfiguration.ApiKey = _stripeEntity.SecretKey;
    }

    public async Task<String> Checkout(int orderId)
    {
        if (orderId <= 0)
            throw new ArgumentException("Order Id must be greater than zero");
        
        var order = await _orderRepository.GetCompleteOrderAsync(orderId);

        if (order is null)
            throw new CustomNotFoundException($"Order with Id {orderId} was not found");

        var options = new SessionCreateOptions()
        {
            Mode = "payment",
            Currency = "usd",
            PaymentMethodTypes = new List<String>()
            {
                "card"
            },
            Metadata = new Dictionary<string, string>()
            {
                { "orderId", orderId.ToString() }
            },
            SuccessUrl = "https://pressstart-sv.vercel.app/success",
            CancelUrl = "https://pressstart-sv.vercel.app/cancel",
            LineItems = order.Details.Select(d => new SessionLineItemOptions()
            {
                Quantity = d.Quantity,
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    Currency = "usd",
                    UnitAmount = (long)(d.Price * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = d.Product?.Name,
                        Description = d.Product?.Description
                    }
                }
            }).ToList()
        };

        var service = new SessionService();
        var checkoutSession = await service.CreateAsync(options);

        var payment = new Payment()
        {
            Order = order,
            OrderId = orderId,
            StripeSessionId = checkoutSession.Id,
            StripeIntentId = String.Empty,
            Amount = (decimal)checkoutSession.AmountTotal / 100,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
        
        var createdPayment = await _paymentRepository.AddAsync(payment);
        
        return checkoutSession.Url;
    }

    public async Task<bool> WebHook()
    {
        var request = _contextAccessor.HttpContext.Request;
        var json = await new StreamReader(request.Body).ReadToEndAsync();

        var webhookSecret =  Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

        var stripeEvent = EventUtility.ConstructEvent
            (json, request.Headers["Stripe-Signature"], webhookSecret);

        if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
        {
            var session = stripeEvent.Data.Object as Session;
            
            var orderIdStr = session?.Metadata["orderId"];
            var orderId = int.Parse(orderIdStr);
            
            var payment = await _paymentRepository.GetByStripeSessionIdAsync(session.Id);

            if (payment.Status == PaymentStatus.Pending)
            {
                payment.Status = PaymentStatus.Success;
                payment.StripeIntentId = session.PaymentIntentId;
                payment.PaymentDate = DateTime.UtcNow;
                payment.UpdateAt = DateTime.UtcNow;

                var order = await _orderRepository.GetCompleteOrderAsync(orderId);

                order.Status = OrderStatus.Paid;
                order.UpdateAt = DateTime.UtcNow;

                await _paymentRepository.UpdateAsync(payment);
                await _orderRepository.UpdateAsync(order);

                await _invoiceService.CreateInvoice(order, payment);
            }
        }
        
        return true;
    }
}