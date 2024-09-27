using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class RecipeService : IRecipeService
	{
		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IIngredientRepository _ingredientRepository;
		private readonly IMapper _mapper;

		public RecipeService(IRecipeRepository repository, ICategoryRepository categoryRepository, IIngredientRepository ingredientRepository, IMapper mapper)
		{
			_recipeRepository = repository;
			_categoryRepository = categoryRepository;
			_ingredientRepository = ingredientRepository;
			_mapper = mapper;
		}

		public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequestDto model)
		{
			var categories = await _categoryRepository
				.GetCategoriesByIdsAsync(model.CategoryIds);
			var recipe = _mapper.Map<Recipe>(model);
			recipe.Categories = categories;
			recipe = await _recipeRepository.CreateAsync(recipe);
			return _mapper.Map<RecipeDto>(recipe);
		}

		public async Task<ICollection<RecipeDto>> GetAllRecipesAsync()
		{
			var recipes = await _recipeRepository.GetAllAsync();
			return _mapper.Map<ICollection<RecipeDto>>(recipes);
		}

		public async Task<RecipeDto> GetRecipeByIdAsync(Guid id)
		{
			var recipe = await _recipeRepository.GetByIdAsync(id);
			return _mapper.Map<RecipeDto>(recipe);
		}

		public async Task<RecipeDto> GetRecipeWithDetailsByIdAsync(Guid id)
		{
			var recipe = await _recipeRepository.GetRecipeWithDetailsById(id);
			return _mapper.Map<RecipeDto>(recipe);
		}

		public async Task<RecipeDto> UpdateRecipeAsync(Guid id, UpdateRecipeRequestDto model)
		{
			var existingRecipe = await _recipeRepository.GetRecipeWithDetailsById(id);
			if (existingRecipe == null)
			{
				return null;
			}

			existingRecipe.Categories.Clear();
			existingRecipe.RecipeIngredients.Clear();
			_mapper.Map(model, existingRecipe);

			foreach (var categoryId in model.CategoryIds)
			{
				var category = await _categoryRepository.GetByIdAsync(categoryId);
				if (category != null)
				{
					existingRecipe.Categories.Add(category);
				}
			}

			foreach (var ingredient in model.Ingredients)
			{
				var newIngredient = await _ingredientRepository.GetByIdAsync(ingredient.IngredientId);
				if (newIngredient != null)
				{
					existingRecipe.RecipeIngredients.Add(new RecipeIngredient
					{
						IngredientId = newIngredient.Id,
						Quantity = ingredient.Quantity
					});
				}
			}

			await _recipeRepository.UpdateAsync(existingRecipe);
			return _mapper.Map<RecipeDto>(existingRecipe);
		}

		public async Task<RecipeDto> DeleteRecipeAsync(Guid id)
		{
			var recipe = await _recipeRepository.DeleteByIdAsync(id);
			return _mapper.Map<RecipeDto>(recipe);
		}
	}
}
