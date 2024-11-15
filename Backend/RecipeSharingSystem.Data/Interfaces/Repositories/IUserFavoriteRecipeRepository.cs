using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Core.Interfaces.Repositories;

public interface IUserFavoriteRecipeRepository
{
	Task<ICollection<Recipe>> GetFavoriteRecipesForUserAsync(Guid userId);
	Task AddRecipeToFavorites(Guid userId, Guid recipeId);
	Task RemoveRecipeFromFavorites(Guid userId, Guid recipeId);
	Task<bool> IsFavorite(Guid userId, Guid recipeId);
}
