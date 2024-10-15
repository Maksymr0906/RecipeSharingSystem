using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Core.Repositories;

public interface IAbstractRepository<TEntity> where TEntity : BaseEntity
{
	Task<TEntity> CreateAsync(TEntity entity);
	Task<ICollection<TEntity>> GetAllAsync();
	Task<TEntity> GetByIdAsync(Guid id);
	Task<TEntity> UpdateAsync(TEntity entity);
	Task<TEntity> DeleteByIdAsync(Guid id);
}