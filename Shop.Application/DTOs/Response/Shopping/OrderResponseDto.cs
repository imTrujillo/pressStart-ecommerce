using Shop.Application.DTOs.Request.Shopping;
using Shop.Domain.Enums;

namespace Shop.Application.DTOs.Response.Shopping;

public record OrderResponseDto
{
    public OrderResponseDto()
    {
        Details = new HashSet<ProductDetailDto>();
    }
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public OrderStatus Status { get; init; }
    public float Total { get; init; }
    public ICollection<ProductDetailDto> Details { get; init; } 
}