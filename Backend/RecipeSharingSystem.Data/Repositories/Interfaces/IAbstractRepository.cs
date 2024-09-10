using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Data.Repositories.Interfaces
{
	public interface IAbstractRepository<TEntity> where TEntity : BaseEntity
	{
		Task<TEntity> CreateAsync(TEntity entity);
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> GetByIdAsync(Guid id);
		Task<TEntity> UpdateAsync(TEntity entity);
		Task<TEntity> DeleteByIdAsync(Guid id);
	}
}
