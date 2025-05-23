using OurTime.Domain.Entities;
using OurTime.Domain.Interfaces;

namespace OurTime.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for managing Product entities.
/// This class inherits from the generic Repository class and adds product-specific functionality.
/// </summary>
public class WatchRepository : Repository<Watch, int>, IWatchRepository
{
    /// <summary>
    /// Constructor that passes the context to the base Repository class
    /// </summary>
    /// <param name="context">The database context</param>
    public WatchRepository(AppDbContext context) : base(context)
    {
    }

    // You can add product-specific methods here if needed
}