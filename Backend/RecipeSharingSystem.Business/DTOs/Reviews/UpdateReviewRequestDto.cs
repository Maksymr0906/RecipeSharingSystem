namespace RecipeSharingSystem.Business.DTOs.Review;

public record UpdateReviewRequestDto
{
	public required int Rating { get; set; }
	public string? Content { get; set; }
}