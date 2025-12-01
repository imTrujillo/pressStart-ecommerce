namespace Shop.Application.DTOs.Response.Shopping;

public record ProductDetailDto
{
    public string ProductName { get; init; }
    public int Quantity { get; init; }
    public float UnitPrice { get; init; }
    public float SubTotal { get; init; }
}