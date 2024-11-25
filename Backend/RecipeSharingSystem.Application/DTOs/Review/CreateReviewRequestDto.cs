namespace RecipeSharingSystem.Business.DTOs.Review;

public record CreateReviewRequestDto
{
	public required int Rating { get; set; }
	public string? Content { get; set; }
	public required Guid UserId { get; set; }
	public required Guid RecipeId { get; set; }
}