using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Persistence.Repositories;

public class UserFavoriteRecipeRepository(RecipeSharingSystemDbContext context)
	: IUserFavoriteRecipeRepository
{
	private readonly DbSet<UserFavoriteRecipe> _entities = context.Set<UserFavoriteRecipe>();

	public async Task AddRecipeToFavorites(Guid userId, Guid recipeId)
	{
		var favorite = new UserFavoriteRecipe
		{
			UserId = userId,
			RecipeId = recipeId
		};

		await _entities.AddAsync(favorite);
	}

	public async Task<ICollection<Recipe>> GetFavoriteRecipesForUserAsync(Guid userId)
	{
		return await _entities
			.Include(f => f.Recipe)
			.ThenInclude(r => r.Categories)
			.Include(f => f.Recipe.Reviews)
			.Where(f => f.UserId == userId)
			.Select(f => f.Recipe)
			.ToListAsync();
	}

	public async Task<bool> IsFavorite(Guid userId, Guid recipeId)
	{
		return await _entities
			.AnyAsync(f => f.UserId == userId && f.RecipeId == recipeId);
	}

	public async Task RemoveRecipeFromFavorites(Guid userId, Guid recipeId)
	{
		var favorite = await _entities
			.FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);

		if (favorite == null)
		{
			throw new KeyNotFoundException("Favorite recipe not found.");
		}

		_entities.Remove(favorite);
	}
}
