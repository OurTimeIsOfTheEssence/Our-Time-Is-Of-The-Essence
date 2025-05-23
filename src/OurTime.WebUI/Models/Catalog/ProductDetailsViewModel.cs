namespace OurTime.WebUI.Models.Catalog;

public class ProductDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Model { get; set; } = string.Empty;

    public bool HasImage => !string.IsNullOrEmpty(ImageUrl);    
    public decimal Price { get; set; } = 0;
    public string? ImageUrl { get; set; }
    public string Description { get; set; } = string.Empty;
}