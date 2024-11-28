using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeSharingSystem.Business.DTOs.Review;
using RecipeSharingSystem.Business.Services.Interfaces;

namespace RecipeSharingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(IReviewService service)
	: ControllerBase
{
	private readonly IReviewService _service = service;

	[Authorize(Policy = "CreateReviewPolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequestDto request)
	{
		try
		{
			await _service.CreateReviewAsync(request);

			return Ok();
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
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

	[Authorize(Policy = "UpdateReviewPolicy")]
	[HttpPut("{id:Guid}")]
	public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] UpdateReviewRequestDto request)
	{
		try
		{
			var review = await _service.UpdateReviewAsync(id, request);

			return Ok(review);
		}
		catch (ValidationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[Authorize(Policy = "DeleteReviewPolicy")]
	[HttpDelete("{id:Guid}")]
	public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
	{
		var review = await _service.DeleteReviewAsync(id);
		if (review == null)
		{
			return NotFound();
		}

		return Ok(review);
	}
}

