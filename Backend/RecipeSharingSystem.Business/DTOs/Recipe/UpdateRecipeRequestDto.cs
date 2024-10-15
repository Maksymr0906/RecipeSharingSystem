namespace RecipeSharingSystem.Business.DTOs.Recipe;

public record UpdateRecipeRequestDto(
	string Title,
	string ShortDescription,
	string FeaturedImageUrl,
	Guid InstructionId,
	IEnumerable<Guid> CategoryIds,
	ICollection<IngredientQuantityDto> Ingredients
);
