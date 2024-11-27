using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class ReviewRepository(RecipeSharingSystemDbContext context)
	: Repository<Review>(context), IReviewRepository
{
	public async Task<Review> GetByUserAndRecipeId(Guid userId, Guid recipeId)
	{
		var review = await GetBaseQuery()
			.FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId);

		if (review == null)
		{
			throw new KeyNotFoundException($"No review found for user {userId} and recipe {recipeId}");
		}

		return review;
	}
	public async Task<ICollection<Review>> GetAllByRecipeId(Guid recipeId)
	{
		try
		{
			return await GetBaseQuery()
				.Where(r => r.RecipeId == recipeId)
				.ToListAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error retrieving reviews: {ex.Message}", ex);
		}
	}

	public async Task<ICollection<Review>> GetAllWithDetails()
	{
		try
		{
			return await GetBaseQuery().ToListAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error retrieving reviews: {ex.Message}", ex);
		}
	}

	public async Task<Review> GetByIdWithDetails(Guid id)
	{
		var review = await GetBaseQuery().FirstOrDefaultAsync(r => r.Id == id);

		if (review == null)
		{
			throw new KeyNotFoundException($"No review found with ID: {id}");
		}

		return review;
	}

	private IQueryable<Review> GetBaseQuery()
	{
		return Entities
			.AsNoTracking()
			.Include(r => r.User)
			.Include(r => r.Recipe);
	}
}
