using RecipeSharingSystem.Business.DTOs.Recipe;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IRecipeService
{
	Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequestDto model);
	Task<ICollection<RecipeDto>> GetAllRecipesAsync();
	Task<RecipeDto> GetRecipeByIdAsync(Guid id);
	Task<ICollection<RecipeDto>> GetRandomRecipesWithDetailsAsync(int numberOfRecipes);
	Task<ICollection<RecipeDto>> GetRecipesByCategoryId(Guid categoryId);
	Task<ICollection<RecipeDto>> GetRecipesByAuthorId(Guid authorId);
	Task<RecipeDto> UpdateRecipeAsync(Guid id, UpdateRecipeRequestDto model);
	Task<RecipeDto> DeleteRecipeAsync(Guid id);
	Task<RecipeDto> GetRecipeBySlugAsync(string slug);
	Task<ICollection<RecipeDto>> SearchRecipesAsync(string query);
}
