namespace Shop.Application.DTOs.Request.Shopping;

public record CreateCategoryDto
{
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsActive { get; set; }
}