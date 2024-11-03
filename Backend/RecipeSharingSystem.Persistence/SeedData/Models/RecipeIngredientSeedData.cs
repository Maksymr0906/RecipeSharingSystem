namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record RecipeIngredientSeedData
{
	public required Guid RecipeId { get; init; }
	public required Guid IngredientId { get; init; }
	public required double Quantity { get; init; }
	public required string MeasurementUnit { get; init; }
}
