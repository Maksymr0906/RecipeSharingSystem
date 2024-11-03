namespace RecipeSharingSystem.Business.DTOs.Rating;

public record UserRecipeRatingRequestDto(
	Guid recipeId,
	Guid userId
);
