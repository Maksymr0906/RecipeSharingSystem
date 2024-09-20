using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Data.Repositories.Implementation
{
	public class AbstractRepository<TEntity> : IAbstractRepository<TEntity> where TEntity : BaseEntity
	{
		private readonly RecipeSharingSystemDbContext _context;
		private readonly DbSet<TEntity> _entities;

		public AbstractRepository(RecipeSharingSystemDbContext context)
		{
			_context = context;
			_entities = _context.Set<TEntity>();
		}

		protected DbSet<TEntity> Entities => _entities;

		public async Task<TEntity> CreateAsync(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			await _entities.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<TEntity> DeleteByIdAsync(Guid id)
		{
			var entity = await _entities.FindAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException();
			}

			_entities.Remove(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<ICollection<TEntity>> GetAllAsync()
		{
			return await _entities.ToListAsync();
		}

		public async Task<TEntity> GetByIdAsync(Guid id)
		{
			var entity = await _entities.FindAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException();
			}

			return entity;
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			var existingEntity = await _entities.FindAsync(entity.Id);
			if (existingEntity == null)
			{
				throw new KeyNotFoundException();
			}

			_context.Entry(existingEntity).CurrentValues.SetValues(entity);
			await _context.SaveChangesAsync();
			return entity;
		}
	}
}
