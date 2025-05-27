using OurTime.Domain.Common;
using OurTime.Domain.ValueObjects;

namespace OurTime.Domain.Entities;

public class Watch : Entity<int>
{
    public int? ExternalProductId { get; set; }
    public int CustomerId { get; private set; }

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

    public Watch(
        int? externalProductId,
        int customerId,
        string name,
        string imageUrl,
        decimal price,
        string model,
        string description,
        string gender,
        int stock,
        bool isCustom = false
    )
    {
        ExternalProductId = externalProductId;
        CustomerId = customerId;
        Name = name;
        ImageUrl = imageUrl;
        Price = price;
        Model = model;
        Description = description;
        Gender = gender;
        Stock = stock;
        IsCustom = isCustom;
    }

    // -------- NY ÖVERLASTNING: bara kärnfält —--------
    public Watch(
        string name,
        string imageUrl,
        decimal price,
        string model,
        string description
    ) : this(
        externalProductId: null,
        customerId: 0,
        name: name,
        imageUrl: imageUrl,
        price: price,
        model: model,
        description: description,
        gender: string.Empty,
        stock: 0,
        isCustom: false
    )
    { }

    /// <summary>
    /// Enkel ctor bara för seed/test.
    /// </summary>
    public Watch(
      string name,
      string description,
      string imageUrl
    ) : this(
        externalProductId: null,
        customerId: 0,
        name: name,
        imageUrl: imageUrl,
        price: 0m,
        model: string.Empty,
        description: description,
        gender: string.Empty,
        stock: 0,
        isCustom: false
    )
    { }
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