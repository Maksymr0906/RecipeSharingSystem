namespace RecipeSharingSystem.Business.DTOs.Recipe;

public record RecipeDto(
	Guid Id,
	string Title,
	string ShortDescription,
	string FeaturedImageUrl,
	Guid InstructionId,
	IEnumerable<Guid> RatingIds,
	IEnumerable<Guid> CategoryIds,
	ICollection<IngredientQuantityDto> Ingredients
);
