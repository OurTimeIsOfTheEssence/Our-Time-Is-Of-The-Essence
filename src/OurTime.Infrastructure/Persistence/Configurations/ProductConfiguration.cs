using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OurTime.Domain.Entities;

namespace OurTime.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration class for the Product entity.
/// This defines how a Product is mapped to the database.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Watch>
{
    /// <summary>
    /// Configures the entity mapping using EF Core's Fluent API.
    /// </summary>
    /// <param name="builder">Entity type builder used to configure the entity</param>
    public void Configure(EntityTypeBuilder<Watch> builder)
    {
        // Define the table name explicitly
        builder.ToTable("Watches");

        // Configure the primary key
        builder.HasKey(p => p.Id);

        // Configure Name property
        builder.Property(p => p.Name)
            .IsRequired() // NOT NULL constraint
            .HasMaxLength(100); // VARCHAR(100)

        // Configure Description property
        builder.Property(p => p.Description)
            .IsRequired() // NOT NULL constraint
            .HasMaxLength(500); // VARCHAR(500)

        // Configure ImageUrl property - it's nullable
        builder.Property(p => p.ImageUrl)
            .IsRequired();

        // Add an index on the Name for faster lookups
        builder.HasIndex(p => p.Name);

        builder.Property(p => p.Price)
            .IsRequired();

        builder.Property(p => p.IsCustom)
            .IsRequired(false);

        builder.Property(p => p.Gender)
            .IsRequired(false);
    }
}