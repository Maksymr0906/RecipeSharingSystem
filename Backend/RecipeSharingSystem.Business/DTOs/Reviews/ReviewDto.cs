namespace RecipeSharingSystem.Business.DTOs.Review;

public record ReviewDto(
	Guid Id,
	string Content,
	DateTime DateCreated,
	Guid UserId,
	Guid RecipeId
);