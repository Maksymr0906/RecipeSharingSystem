using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Application.DTOs.Reviews;
using RecipeSharingSystem.Business.DTOs.Review;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewService _service;

		public ReviewController(IReviewService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequestDto request)
		{
			await _service.CreateReviewAsync(request);
			return Ok();
		}

		[HttpGet("recipes/{recipeId}/users/{userId}")]
		public async Task<ActionResult> GetUserRecipeReview([FromBody] UserRecipeReviewRequestDto request)
		{
			var review = await _service.GetUserRecipeReview(request);
			if (review == null)
			{
				return NotFound();
			}

			return Ok(review);
		}
	}
}

