namespace RecipeSharingSystem.Business.DTOs.Review;

public record CreateReviewRequestDto
{
	public required int Rating { get; set; }
	public string? Content { get; set; }
	// Ignored by the server: the reviewer is always the authenticated caller. Kept for client compatibility.
	public Guid UserId { get; set; }
	public required Guid RecipeId { get; set; }
}