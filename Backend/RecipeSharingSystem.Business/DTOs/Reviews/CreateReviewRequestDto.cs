namespace RecipeSharingSystem.Business.DTOs.Review;

public record CreateReviewRequestDto(
	string Content,
	Guid UserId,
	Guid RecipeId
);