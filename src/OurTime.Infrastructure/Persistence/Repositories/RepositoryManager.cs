using OurTime.Application.Common.Interfaces;
using OurTime.Domain.Interfaces;

namespace OurTime.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementation of the Repository Manager pattern.
/// This class provides a single point of access to all repositories and the unit of work.
/// </summary>
public class RepositoryManager : IRepositoryManager{
    private readonly IWatchRepository _watchRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Constructor that accepts all required repositories and the unit of work
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="orderRepository">The order repository</param>
    /// <param name="unitOfWork">The unit of work</param>
    public RepositoryManager(IWatchRepository watchRepository, IUnitOfWork unitOfWork)
    {
        _watchRepository = watchRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public IWatchRepository WatchRepository => _watchRepository;


    /// <inheritdoc/>
    public IUnitOfWork UnitOfWork => _unitOfWork;
}