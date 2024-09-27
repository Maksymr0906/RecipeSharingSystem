﻿using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RecipesController : ControllerBase
	{
		private readonly IRecipeService _service;

		public RecipesController(IRecipeService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeRequestDto request)
		{
			var recipe = await _service.CreateRecipeAsync(request);
			return Ok(recipe);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllRecipes()
		{
			var recipes = await _service.GetAllRecipesAsync();
			return Ok(recipes);
		}

		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetRecipeById([FromRoute] Guid id, [FromQuery] bool includeDetails = false)
		{
			var recipe = includeDetails
				? await _service.GetRecipeWithDetailsByIdAsync(id)
				: await _service.GetRecipeByIdAsync(id);

			return Ok(recipe);
		}

		[HttpPut("{id:Guid}")]
		public async Task<IActionResult> UpdateRecipe([FromRoute] Guid id, [FromBody] UpdateRecipeRequestDto request)
		{
			var recipe = await _service.UpdateRecipeAsync(id, request);
			if (recipe == null)
			{
				return NotFound();
			}

			return Ok(recipe);
		}
	}
}
