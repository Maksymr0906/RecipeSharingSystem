﻿using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class RecipeService(
	IUnitOfWork unitOfWork,
	IMapper mapper,
	ICategoryService categoryService,
	IIngredientService ingredientService
	) : IRecipeService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly ICategoryService _categoryService = categoryService;
	private readonly IIngredientService _ingredientService = ingredientService;

	public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequestDto model)
	{ 
		var categories = await _categoryService.GetCategoriesByIdsAsync(model.CategoryIds);
		var recipe = _mapper.Map<Recipe>(model);
		recipe.Categories = categories;

		foreach (var ingredient in model.Ingredients)
		{
			var existingIngredient = await _ingredientService.GetOrCreateIngredientAsync(ingredient.IngredientName);
			recipe.RecipeIngredients.Add(new RecipeIngredient
			{
				IngredientId = existingIngredient.Id,
				RecipeId = recipe.Id,
				Quantity = ingredient.Quantity,
				MeasurementUnit = ingredient.MeasurementUnit
			});
		}

		recipe = await _unitOfWork.RecipeRepository.CreateAsync(recipe);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<RecipeDto>(recipe);
	}


	public async Task<ICollection<RecipeDto>> GetAllRecipesAsync()
	{
		var recipes = await _unitOfWork.RecipeRepository.GetAllWithDetailsAsync();
		return _mapper.Map<ICollection<RecipeDto>>(recipes);
	}

	public async Task<RecipeDto> GetRecipeByIdAsync(Guid id)
	{
		var recipe = await _unitOfWork.RecipeRepository.GetWithDetailsByIdAsync(id);
		return _mapper.Map<RecipeDto>(recipe);
	}

	public async Task<RecipeDto> UpdateRecipeAsync(Guid id, UpdateRecipeRequestDto model)
	{
		var existingRecipe = await _unitOfWork.RecipeRepository.GetWithDetailsByIdAsync(id);
		if (existingRecipe == null)
		{
			return null;
		}

		_mapper.Map(model, existingRecipe);
		
		existingRecipe.Categories.Clear();
		var categories = await _categoryService.GetCategoriesByIdsAsync(model.CategoryIds);
		existingRecipe.Categories = categories;

		foreach (var ingredient in model.Ingredients)
		{
			var existingIngredient = await _ingredientService.GetOrCreateIngredientAsync(ingredient.IngredientName);
			existingRecipe.RecipeIngredients.Add(new RecipeIngredient
			{
				IngredientId = existingIngredient.Id,
				RecipeId = existingRecipe.Id,
				Quantity = ingredient.Quantity,
				MeasurementUnit = ingredient.MeasurementUnit
			});
		}

		existingRecipe = await _unitOfWork.RecipeRepository.UpdateAsync(existingRecipe);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<RecipeDto>(existingRecipe);
	}

	public async Task<RecipeDto> DeleteRecipeAsync(Guid id)
	{
		var recipe = await _unitOfWork.RecipeRepository.DeleteByIdAsync(id);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<RecipeDto>(recipe);
	}

	public async Task<ICollection<RecipeDto>> GetRandomRecipesWithDetailsAsync(int numberOfRecipes)
	{
		var allRecipes = await _unitOfWork.RecipeRepository.GetAllWithDetailsAsync();
		var randomRecipes = allRecipes.OrderBy(r => Guid.NewGuid()).Take(numberOfRecipes).ToList();
		return _mapper.Map<ICollection<RecipeDto>>(randomRecipes);
	}

	public async Task<ICollection<RecipeDto>> GetRecipesByCategoryId(Guid categoryId)
	{
		var recipes = await _unitOfWork.RecipeRepository.GetAllWithDetailsAsync();
		var categoryRecipes = recipes
			.Where(r => r.Categories.Any(c => c.Id == categoryId))
			.ToList();
		return _mapper.Map<ICollection<RecipeDto>>(categoryRecipes);
	}
}