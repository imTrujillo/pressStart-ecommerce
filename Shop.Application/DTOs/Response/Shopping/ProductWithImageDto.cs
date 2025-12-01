namespace Shop.Application.DTOs.Response.Shopping;

public record ProductWithImageDto()
{
    public int ProductId { get; init; }
    public string ProductName { get; init; }
    public string Description { get; init; }
    public float Price  { get; init; }
    public int Stock  { get; init; }
    public string CategoryName { get; init; }
    public string ProviderName { get; init; }
    public List<ImageDto> Images { get; init; }
};