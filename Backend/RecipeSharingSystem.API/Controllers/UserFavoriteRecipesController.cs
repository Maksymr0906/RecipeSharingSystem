using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Application.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/users/{userId:guid}/favorites")]
[ApiController]
public class UserFavoriteRecipes(IUserFavoriteRecipesService service)
		: ControllerBase
{
	private readonly IUserFavoriteRecipesService _service = service;

	[HttpGet]
	public async Task<IActionResult> GetFavoriteRecipes([FromRoute] Guid userId)
	{
		var favorites = await _service.GetFavoriteRecipesForUser(userId);
		return Ok(favorites);
	}

	[HttpPost("recipes/{recipeId:guid}")]
	public async Task<IActionResult> AddToFavorites(Guid userId, Guid recipeId)
	{
		await _service.AddRecipeToFavorites(userId, recipeId);
		return NoContent();
	}

	[HttpDelete("recipes/{recipeId:guid}")]
	public async Task<IActionResult> DeleteFromFavorites(Guid userId, Guid recipeId)
	{
		await _service.RemoveRecipeFromFavorites(userId, recipeId);
		return NoContent();
	}

	[HttpGet("recipes/{recipeId:guid}/status")]
	public async Task<IActionResult> IsRecipeFavorite(Guid userId, Guid recipeId)
	{
		var isFavorite = await _service.IsRecipeFavorite(userId, recipeId);
		return Ok(new { IsFavorite = isFavorite });
	}
}
