namespace Shop.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<String> Checkout(int orderId);
    Task<bool> WebHook();
}