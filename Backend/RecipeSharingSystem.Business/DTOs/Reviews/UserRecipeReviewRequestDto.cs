namespace RecipeSharingSystem.Application.DTOs.Reviews;

public record UserRecipeReviewRequestDto(
	Guid recipeId,
	Guid userId
);
