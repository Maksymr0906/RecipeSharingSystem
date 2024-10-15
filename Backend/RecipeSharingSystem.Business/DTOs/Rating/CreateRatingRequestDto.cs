namespace RecipeSharingSystem.Business.DTOs.Rating;

public record CreateRatingRequestDto(
	int Value,
	Guid UserId,
	Guid RecipeId
);
