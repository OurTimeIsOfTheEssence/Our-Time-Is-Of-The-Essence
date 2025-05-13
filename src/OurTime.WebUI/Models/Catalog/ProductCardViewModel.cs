namespace OurTime.WebUI.Models.Catalog;

public class ProductCardViewModel
{

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Model { get; set; } = string.Empty;

    public bool HasImage => !string.IsNullOrEmpty(ImageUrl);    
    public decimal PriceAmount { get; set; }
    public string? ImageUrl { get; set; }
    public string Description { get; set; } = string.Empty;
}