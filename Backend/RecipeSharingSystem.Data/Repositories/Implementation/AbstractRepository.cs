using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class AbstractRepository<TEntity> : IAbstractRepository<TEntity> where TEntity : BaseEntity
	{
		public Task<TEntity> CreateAsync(TEntity entity)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> DeleteByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TEntity>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> UpdateAsync(TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}
