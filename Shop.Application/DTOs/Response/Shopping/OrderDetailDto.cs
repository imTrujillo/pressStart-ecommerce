namespace Shop.Application.DTOs.Response.Shopping;

public class OrderDetailDto
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public float Price { get; init; }
}