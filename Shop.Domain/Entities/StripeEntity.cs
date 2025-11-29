namespace Shop.Domain.Entities;

public class StripeEntity
{
    public string PublishableKey { get; set; } = String.Empty;
    public string SecretKey { get; set; } = String.Empty;
    public string WebhookSecret { get; set; } = string.Empty;

}