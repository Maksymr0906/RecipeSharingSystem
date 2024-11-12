using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Review;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewsController : ControllerBase
	{
		private readonly IReviewService _service;

		public ReviewsController(IReviewService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequestDto request)
		{
			await _service.CreateReviewAsync(request);
			return Ok();
		}

		[HttpGet("recipes/{recipeId:Guid}/users/{userId:Guid}")]
		public async Task<IActionResult> GetUserReviewForRecipe([FromRoute] Guid recipeId, [FromRoute] Guid userId)
		{
			var review = await _service.GetUserRecipeReview(recipeId, userId);
			if (review == null)
			{
				return NotFound();
			}

			return Ok(review);
		}

		[HttpGet("recipe/{recipeId:Guid}")]
		public async Task<IActionResult> GetRecipeReviews([FromRoute] Guid recipeId)
		{
			var reviews = await _service.GetRecipeReviews(recipeId);
			return Ok(reviews);
		}

		[HttpPut("{id:Guid}")]
		public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] UpdateReviewRequestDto request)
		{
			var rating = await _service.UpdateReviewAsync(id, request);
			return Ok(rating);
		}
	}
}

