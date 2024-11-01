using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
	private readonly RecipeSharingSystemDbContext _context;
	private readonly DbSet<TEntity> _entities;

	public Repository(RecipeSharingSystemDbContext context)
	{
		_context = context;
		_entities = _context.Set<TEntity>();
	}

	protected RecipeSharingSystemDbContext Context => _context;
	protected DbSet<TEntity> Entities => _entities;

	public async Task<TEntity> CreateAsync(TEntity entity)
	{
		if (entity == null)
		{
			throw new ArgumentNullException(nameof(entity));
		}

		await _entities.AddAsync(entity);
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
		_entities.Update(entity);
		return entity;
	}
}
