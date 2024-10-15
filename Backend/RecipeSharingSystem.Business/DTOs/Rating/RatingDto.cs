namespace RecipeSharingSystem.Business.DTOs.Rating;

public record RatingDto(
	Guid Id,
	int Value,
	Guid UserId,
	Guid RecipeId
);
