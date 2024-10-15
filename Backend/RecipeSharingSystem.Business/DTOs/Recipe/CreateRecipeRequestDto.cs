namespace RecipeSharingSystem.Business.DTOs.Recipe;

public record CreateRecipeRequestDto(
	string Title,
	string ShortDescription,
	string FeaturedImageUrl,
	Guid InstructionId,
	IEnumerable<Guid> CategoryIds,
	ICollection<IngredientQuantityDto> Ingredients
);
