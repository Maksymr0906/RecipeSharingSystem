using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class RatingRepository(RecipeSharingSystemDbContext context)
	: Repository<Rating>(context), IRatingRepository
{
	public async Task<Rating> GetByUserAndRecipeId(Guid userId, Guid recipeId)
	{
		return await Entities.FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId);
	}
}
