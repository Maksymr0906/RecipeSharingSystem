using RecipeSharingSystem.Business.DTOs.Ingredient;

namespace RecipeSharingSystem.Business.Services.Interfaces
{
	public interface IIngredientService
	{
		Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequestDto model);
		Task<ICollection<IngredientDto>> GetAllIngredientsAsync();
		Task<IngredientDto> GetIngredientByIdAsync(Guid id);
		Task<IngredientDto> UpdateIngredientAsync(Guid id, UpdateIngredientRequestDto model);
		Task<IngredientDto> DeleteIngredientAsync(Guid id);
	}
}
