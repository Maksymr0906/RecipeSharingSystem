using RecipeSharingSystem.Business.DTOs.Recipe;

namespace RecipeSharingSystem.Application.Services.Interfaces;

public interface IUserFavoriteRecipesService
{
	Task<ICollection<RecipeDto>> GetFavoriteRecipesForUser(Guid userId);
	Task AddRecipeToFavorites(Guid userId, Guid recipeId);
	Task RemoveRecipeFromFavorites(Guid userId, Guid recipeId);
	Task<bool> IsRecipeFavorite(Guid userId, Guid recipeId);
}
