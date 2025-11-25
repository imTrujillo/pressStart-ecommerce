using Shop.Application.DTOs.Response.Shopping;
using Shop.Domain.Entities.ShoppingEntities;
using Shop.Domain.Enums;

namespace Shop.Application.DTOs.Request.Shopping;

public record CreateOrderDto
{
    public CreateOrderDto()
    {
        Details = new List<OrderDetailDto>();
    }
    public int UserId { get; init; }
    public OrderStatus OrderStatus { get; init; } = OrderStatus.Pending;
    public string Address { get; init; }
    public ICollection<OrderDetailDto> Details { get; init; }
};