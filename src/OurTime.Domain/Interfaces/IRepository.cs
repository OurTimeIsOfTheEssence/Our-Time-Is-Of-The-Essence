using OurTime.Domain.Common;

namespace OurTime.Domain.Interfaces;

// Generic repository interface for standard CRUD operations
public interface IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task SaveChangesAsync();
}