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
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    public CatalogService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    /// <inheritdoc/>
    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }
}