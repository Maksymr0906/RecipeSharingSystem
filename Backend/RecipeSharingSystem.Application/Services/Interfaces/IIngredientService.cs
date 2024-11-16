using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IIngredientService
{
	Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequestDto model);
	Task<ICollection<IngredientDto>> GetAllIngredientsAsync();
	Task<IngredientDto> GetIngredientByIdAsync(Guid id);
	Task<IngredientDto> UpdateIngredientAsync(Guid id, UpdateIngredientRequestDto model);
	Task<IngredientDto> DeleteIngredientAsync(Guid id);
	Task<Ingredient> GetOrCreateIngredientAsync(string ingredientName);
	Task<IngredientDto> GetIngredientBySlugAsync(string slug);
}
