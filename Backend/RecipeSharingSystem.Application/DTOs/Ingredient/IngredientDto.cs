namespace RecipeSharingSystem.Business.DTOs.Ingredient;

public record IngredientDto(
	Guid Id,
	string Name,
	string Slug
);
