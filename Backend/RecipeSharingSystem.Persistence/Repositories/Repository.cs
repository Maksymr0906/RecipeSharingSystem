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

		if (await _entities.AnyAsync(e => e.Id == entity.Id))
		{
			throw new InvalidOperationException($"An entity with ID {entity.Id} already exists.");
		}

		try
		{
			await _entities.AddAsync(entity);
			return entity;
		}
		catch (DbUpdateException ex)
		{
			throw new InvalidOperationException($"Error creating entity: {ex.Message}", ex);
		}
	}

	public async Task<TEntity> DeleteByIdAsync(Guid id)
	{
		var entity = await _entities.FindAsync(id);
		if (entity == null)
		{
			throw new KeyNotFoundException($"Entity with ID {id} not found.");
		}

		try
		{
			_entities.Remove(entity);
			return entity;
		}
		catch (DbUpdateException ex)
		{
			throw new InvalidOperationException($"Error deleting entity: {ex.Message}", ex);
		}
	}

	public async Task<ICollection<TEntity>> GetAllAsync()
	{
		try
		{
			return await _entities.AsNoTracking().ToListAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error retrieving entities: {ex.Message}", ex);
		}
	}

	public async Task<TEntity> GetByIdAsync(Guid id)
	{
		var entity = await _entities.FindAsync(id);

		if (entity == null)
		{
			throw new KeyNotFoundException($"Entity with ID {id} not found.");
		}

		return entity;
	}

	public async Task<TEntity> UpdateAsync(TEntity entity)
	{
		if (entity == null)
		{
			throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
		}

		var existingEntity = await _entities.FindAsync(entity.Id);
		if (existingEntity == null)
		{
			throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");
		}

		try
		{
			_entities.Update(entity);
			return entity;
		}
		catch (DbUpdateException ex)
		{
			throw new InvalidOperationException($"Error updating entity: {ex.Message}", ex);
		}
	}
}
