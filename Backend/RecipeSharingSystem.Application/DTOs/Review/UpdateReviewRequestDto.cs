namespace RecipeSharingSystem.Business.DTOs.Review;

public record UpdateReviewRequestDto
{
	public int Rating { get; set; }
	public string? Content { get; set; }
}