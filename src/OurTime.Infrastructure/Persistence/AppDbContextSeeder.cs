using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OurTime.Domain.Entities;
using OurTime.Domain.ValueObjects;

namespace OurTime.Infrastructure.Persistence;

/// <summary>
/// Class for seeding the database with initial data.
/// This is useful for development, testing, and demos.
/// </summary>
public class AppDbContextSeeder
{
    private readonly ILogger<AppDbContextSeeder> _logger;
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor that accepts the context and a logger
    /// </summary>
    /// <param name="context">The database context to seed</param>
    /// <param name="logger">The logger for logging seed operations</param>
    public AppDbContextSeeder(AppDbContext context, ILogger<AppDbContextSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public virtual async Task SeedAsync()
    {
        try
        {
            // Ensure the database is created (only needed for in-memory database)
            // For SQL Server, you would use migrations instead
            await _context.Database.EnsureCreatedAsync();

            // Seed products if none exist
            await SeedProductsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    /// <summary>
    /// Seeds the database with sample products
    /// </summary>
    private async Task SeedProductsAsync()
    {
        // Check if we already have products (to avoid duplicate seeding)
        if (!await _context.Watches.AnyAsync())
        {
            _logger.LogInformation("Seeding products...");

            // Add sample products
            var watches = new List<Watch>
            {
                new Watch(
                    name: "Rolex Submariner",
                    description: "A classic diving watch with a timeless design.",
                    imageUrl: new string("https://storageaccountblobb.blob.core.windows.net/images/Blossom.png")
                )

            };

            await _context.Watches.AddRangeAsync(watches);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product seeding completed successfully.");
        }
        else
        {
            _logger.LogInformation("Database already contains products. Skipping product seed.");
        }
    }
}