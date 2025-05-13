using OurTime.Domain.Entities;

namespace OurTime.Domain.Interfaces;

public interface IWatchRepository : IRepository<Watch, int>
{
    // You can add product-specific methods here if needed
}