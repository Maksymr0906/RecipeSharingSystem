﻿using RecipeSharingSystem.Business.DTOs.Recipe;

namespace RecipeSharingSystem.Business.Services.Interfaces
{
	public interface IRecipeService
	{
		Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequestDto model);
		Task<ICollection<RecipeDto>> GetAllRecipesAsync();
		Task<RecipeDto> GetRecipeByIdAsync(Guid id);
		Task<RecipeDto> GetRecipeWithDetailsByIdAsync(Guid id);
		Task<RecipeDto> UpdateRecipeAsync(Guid id, UpdateRecipeRequestDto model);
		Task<RecipeDto> DeleteRecipeAsync(Guid id);
	}
}
