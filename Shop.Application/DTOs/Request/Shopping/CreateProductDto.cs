namespace Shop.Application.DTOs.Request.Shopping;

public record CreateProductDto(
    string ProductName,
    string Description,
    float Price,
    int Stock,
    int CategoryId,
    int ProviderId
);