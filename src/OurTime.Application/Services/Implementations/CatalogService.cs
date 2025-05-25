using OurTime.Application.Services.Interfaces;
using OurTime.Domain.Entities;
using OurTime.Domain.Interfaces;

namespace OurTime.Application.Services.Implementations;

/// <summary>
/// Implementation of the catalog service.
/// Acts as a facade over the repository layer.
/// </summary>
public class CatalogService : ICatalogService
{
    private readonly IWatchRepository _watchRepository;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    /// <param name="watchRepository">The watch repository</param>
    public CatalogService(IWatchRepository watchRepository)
    {
        _watchRepository = watchRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Watch>> GetAllWatchesAsync()
    {
        return await _watchRepository.GetAllAsync();
    }

    /// <inheritdoc/>
    public async Task<Watch?> GetWatchByIdAsync(int id)
    {
        return await _watchRepository.GetByIdAsync(id);
    }

    public async Task AddWatchAsync(Watch watch)
    {
        await _watchRepository.AddAsync(watch);
        await _watchRepository.SaveChangesAsync();
    }

    public async Task DeleteWatchAsync(Watch watch)
    {

            await _watchRepository.RemoveAsync(watch);
            await _watchRepository.SaveChangesAsync();
    }   

}