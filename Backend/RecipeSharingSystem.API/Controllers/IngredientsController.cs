using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientsController(IIngredientService service)
	: ControllerBase
{
	private readonly IIngredientService _service = service;

	[Authorize(Policy = "CreatePolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateIngredient([FromBody] CreateIngredientRequestDto request)
	{
		var ingredient = await _service.CreateIngredientAsync(request);
		return Ok(ingredient);
	}

	[HttpGet]
	public async Task<IActionResult> GetAllIngredients()
	{
		var ingredients = await _service.GetAllIngredientsAsync();
		return Ok(ingredients);
	}

	[HttpGet("{id:Guid}")]
	public async Task<IActionResult> GetIngredientById([FromRoute] Guid id)
	{
		var ingredient = await _service.GetIngredientByIdAsync(id);
		if (ingredient == null)
		{
			return NotFound();
		}

		return Ok(ingredient);
	}

	[Authorize(Policy = "UpdatePolicy")]
	[HttpPut("{id:Guid}")]
	public async Task<IActionResult> UpdateIngredient([FromRoute] Guid id, [FromBody] UpdateIngredientRequestDto request)
	{
		var ingredient = await _service.UpdateIngredientAsync(id, request);
		if (ingredient == null)
		{
			return NotFound();
		}

		return Ok(ingredient);
	}

	[Authorize(Policy = "DeletePolicy")]
	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> DeleteIngredient([FromRoute] Guid id)
	{
		var ingredient = await _service.DeleteIngredientAsync(id);
		if (ingredient == null)
		{
			return NotFound();
		}

		return Ok(ingredient);
	}

	[HttpGet("slug/{slug}")]
	public async Task<IActionResult> GetIngredientBySlug([FromRoute] string slug)
	{
		var ingredient = await _service.GetIngredientBySlugAsync(slug);
		if (ingredient == null)
		{
			return NotFound();
		}

		return Ok(ingredient);
	}
}
