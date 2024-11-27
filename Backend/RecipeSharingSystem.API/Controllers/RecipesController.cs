using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipesController(IRecipeService service)
	: ControllerBase
{
	private readonly IRecipeService _service = service;

	[Authorize(Policy = "CreateRecipePolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeRequestDto request)
	{
		try
		{
			var recipe = await _service.CreateRecipeAsync(request);
			return Ok(recipe);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetAllRecipes()
	{
		var recipes = await _service.GetAllRecipesAsync();
		return Ok(recipes);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetRecipeById([FromRoute] Guid id)
	{
		var recipe = await _service.GetRecipeByIdAsync(id);
		return Ok(recipe);
	}

	[HttpGet("random")]
	public async Task<IActionResult> GetRandomRecipes([FromQuery] int count = 5)
	{
		var randomRecipes = await _service.GetRandomRecipesWithDetailsAsync(count);
		return Ok(randomRecipes);
	}

	[HttpGet("category/{categoryId:Guid}")]
	public async Task<IActionResult> GetRecipesByCategoryId([FromRoute] Guid categoryId)
	{
		var recipes = await _service.GetRecipesByCategoryId(categoryId);
		return Ok(recipes);
	}

	[Authorize(Policy = "UpdateRecipePolicy")]
	[HttpPut("{id:Guid}")]
	public async Task<IActionResult> UpdateRecipe([FromRoute] Guid id, [FromBody] UpdateRecipeRequestDto request)
	{
		try
		{
			var recipe = await _service.UpdateRecipeAsync(id, request);
			if (recipe == null)
			{
				return NotFound();
			}

			return Ok(recipe);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[Authorize(Policy = "DeleteRecipePolicy")]
	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> DeleteRecipe([FromRoute] Guid id)
	{
		var recipe = await _service.DeleteRecipeAsync(id);
		if (recipe == null)
		{
			return NotFound();
		}

		return Ok(recipe);
	}

	[HttpGet("slug/{slug}")]
	public async Task<IActionResult> GetRecipeBySlug([FromRoute] string slug)
	{
		var recipe = await _service.GetRecipeBySlugAsync(slug);
		if (recipe == null)
		{
			return NotFound();
		}

		return Ok(recipe);
	}

	[HttpGet("search")]
	public async Task<IActionResult> SearchRecipes([FromQuery] string query)
	{
		if (string.IsNullOrWhiteSpace(query))
		{
			return BadRequest("Search query cannot be empty");
		}

		var response = await _service.SearchRecipesAsync(query);
		return Ok(response);
	}
}
