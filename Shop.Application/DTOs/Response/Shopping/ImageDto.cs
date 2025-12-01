namespace Shop.Application.DTOs.Response.Shopping;

public record ImageDto()
{
    public int ImageId { get; init; }
    public string FileName { get; init; } = String.Empty;
    public string ImageUrl { get; init; } = String.Empty;
    public bool IsMain { get; init; }
    public int Order { get; init; }
};