using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class ReviewRepository(RecipeSharingSystemDbContext context)
	: Repository<Review>(context), IReviewRepository
{
	public async Task<Review> GetByUserAndRecipeId(Guid userId, Guid recipeId)
	{
		var review = await Entities
			.Include(r => r.User)
			.Include(r => r.Recipe)
			.FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId);

		if (review == null)
		{
			throw new KeyNotFoundException();
		}

		return review;
	}
	public async Task<ICollection<Review>> GetAllByRecipeId(Guid recipeId)
	{
		return await Entities
			.Include(r => r.User)
			.Include(r => r.Recipe)
			.Where(r => r.RecipeId == recipeId)
			.ToListAsync();
	}

	public async Task<ICollection<Review>> GetAllWithDetails()
	{
		return await Entities
			.Include(r => r.User)
			.Include(r => r.Recipe)
			.ToListAsync();
	}

	public async Task<Review> GetByIdWithDetails(Guid id)
	{
		var review = await Entities
			.Include(r => r.User)
			.Include(r => r.Recipe)
			.FirstOrDefaultAsync(r => r.Id == id);

		if (review == null)
		{
			throw new KeyNotFoundException();
		}

		return review;
	}
}
