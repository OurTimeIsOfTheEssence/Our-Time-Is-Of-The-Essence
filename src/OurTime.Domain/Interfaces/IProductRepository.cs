using OurTime.Domain.Entities;

namespace OurTime.Domain.Interfaces;

public interface IProductRepository : IRepository<Product, Guid>
{
    // You can add product-specific methods here if needed
}