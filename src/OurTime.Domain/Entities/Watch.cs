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

    public decimal? Price { get; set; } = null!;

    private Watch()
    {

    }

    public Watch(string name, string? imageUrl, decimal? price = null, string? model = null, string? description = null)
    {
        Name = name;
        ImageUrl = imageUrl;
        Price = price;
        Model = model;
        Description = description;
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