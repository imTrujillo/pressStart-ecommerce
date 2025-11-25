using Microsoft.AspNetCore.Http;

namespace Shop.Application.DTOs.Request.Shopping;

public record ImageRequest
{
    public IFormFile Image { get; init; } =  default!;
}