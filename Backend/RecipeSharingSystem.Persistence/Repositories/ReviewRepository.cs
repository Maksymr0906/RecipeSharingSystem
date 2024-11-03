using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class ReviewRepository(RecipeSharingSystemDbContext context)
	: Repository<Review>(context), IReviewRepository
{
	public async Task<Review> GetByUserAndRecipeId(Guid userId, Guid recipeId)
	{
		return await Entities.FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId);
	}
}
