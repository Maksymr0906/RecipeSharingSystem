using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Rating;
using RecipeSharingSystem.Business.Services.Implementation;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatingsController : ControllerBase
	{
		private readonly IRatingService _service;

		public RatingsController(IRatingService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequestDto request)
		{
			await _service.CreateRatingAsync(request);
			return Ok();
		}

		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetRatingById([FromRoute] Guid id)
		{
			var response = await _service.GetRatingByIdAsync(id);
			return Ok(response);
		}

		[HttpGet("recipes/{recipeId}/users/{userId}")]
		public async Task<ActionResult> GetUserRecipeRating([FromBody] UserRecipeRatingRequestDto request)
		{
			var rating = await _service.GetUserRecipeRatingAsync(request);
			if (rating == null)
			{
				return NotFound();
			}

			return Ok(rating);
		}
	}
}
