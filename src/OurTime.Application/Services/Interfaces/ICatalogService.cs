using OurTime.Domain.Entities;

namespace OurTime.Application.Services.Interfaces;

/// <summary>
/// Service interface for Catalog-related operations.
/// Provides a simple abstraction over the repository layer.
/// </summary>
public interface ICatalogService
{
    /// <summary>
    /// Gets all available products
    /// </summary>
    /// <returns>A collection of all products</returns>
    Task<IEnumerable<Watch>> GetAllWatchesAsync();

    /// <summary>
    /// Gets a product by its unique identifier
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <returns>The product if found, null otherwise</returns>
    Task<Watch?> GetWatchByIdAsync(int id);

    Task AddWatchAsync(Watch watch);
    Task DeleteWatchAsync(Watch watch);
}