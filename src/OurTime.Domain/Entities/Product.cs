using OurTime.Domain.Common;
using OurTime.Domain.ValueObjects;

namespace OurTime.Domain.Entities;

public class Product : Entity<Guid>
{
    // Properties with private setters for encapsulation
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.FromSEK(0);
    public int StockQuantity { get; private set; } = 0;
    public Uri? ImageUrl { get; private set; } = null;

    // Private parameterless constructor for EF Core
    private Product()
    {
        // Required for EF Core, but we don't want it to be used directly
    }

    // Public constructor with required parameters
    public Product(string name, string description, Uri? imageUrl, Money price, int stockQuantity) : base(Guid.NewGuid())
    {

        // Set properties
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        StockQuantity = stockQuantity;
    }

    // Domain methods that encapsulate business logic
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

    public void UpdateStock(int quantity)
    {
        StockQuantity = quantity;
    }


    public bool DecrementStock(int quantity = 1)
    {

        StockQuantity -= quantity;
        return true;
    }

    public void IncrementStock(int quantity)
    {

        StockQuantity += quantity;
    }
}