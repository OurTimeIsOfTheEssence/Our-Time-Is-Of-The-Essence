using OurTime.Domain.Common;
using OurTime.Domain.ValueObjects;

namespace OurTime.Domain.Entities;

public class Watch : Entity<int>
{
    public int Id { get; set; }

    public string? Model { get; set; } 

    public string? Description { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? ImageUrl { get; set; } = null!;

    public decimal Price { get; set; } = 0;
    public string? Gender { get; set; } = string.Empty;
    public int? Stock { get; set; } = 0;
    public bool? IsCustom { get; set; } = false;

    private Watch()
    {

    }

    public Watch(string name, string? imageUrl, decimal price = 0, string? model = null, string? description = null, string? gender = "", int stock = 0, bool isCustom = false)
    {
        Name = name;
        ImageUrl = imageUrl;
        Price = price;
        Model = model;
        Description = description;
        Gender = gender;
        Stock = stock;
        IsCustom = isCustom;
        
    }

        public void UpdateDetails(string name, string description, string? imageUrl)
    {

        // Update properties after all validation passes
        Name = name;
        Description = description;
        ImageUrl = imageUrl;  // Assuming the property name has been updated to imageUrl
    }

        public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
    }
}