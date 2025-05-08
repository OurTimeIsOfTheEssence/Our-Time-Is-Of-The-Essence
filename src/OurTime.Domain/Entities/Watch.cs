using OurTime.Domain.Common;
using OurTime.Domain.ValueObjects;

namespace OurTime.Domain.Entities;

public class Watch
{
    public string Id { get; set; } = null!;

    public string? Model { get; set; }

    public string? Description { get; set; }

    public string Name { get; set; } = null!;

    public Uri ImageUrl { get; set; } = null!;

    public Money Price { get; set; }

    private Watch()
    {

    }

    public Watch(string name, Uri imageUrl, Money price, string? model = null, string? description = null)
    {
        Name = name;
        ImageUrl = imageUrl;
        Price = price;
        Model = model;
        Description = description;
    }

        public void UpdateDetails(string name, string description, Uri? imageUrl)
    {

        // Update properties after all validation passes
        Name = name;
        Description = description;
        ImageUrl = imageUrl;  // Assuming the property name has been updated to imageUrl
    }

        public void UpdatePrice(Money newPrice)
    {
        Price = newPrice;
    }
}