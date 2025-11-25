using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Domain.Entities;

public class ImageEntity : BaseEntity
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    
    public string ImageUrl { get; set; } = String.Empty;
    public string ImagePath { get; set; } = String.Empty;
    public string ExtensionType { get; set; } = String.Empty;
    public bool IsMainImage { get; set; }
    public int Order { get; set; }
    public string FileName { get; set; } = String.Empty;
    
    public long BytesSize { get; set; }
}