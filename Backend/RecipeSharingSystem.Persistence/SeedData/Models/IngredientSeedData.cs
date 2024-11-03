namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record IngredientSeedData
{
	public required Guid Id { get; init; }
	public required string Name { get; init; }
	public required string Slug { get; init; }
}
