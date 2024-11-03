namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record RecipeCategorySeedData
{
	public required Guid RecipeId { get; init; }
	public required Guid CategoryId { get; init; }
}
