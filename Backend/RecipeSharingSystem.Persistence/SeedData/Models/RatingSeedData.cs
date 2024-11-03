namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record RatingSeedData
{
	public required Guid Id { get; init; }
	public required int Value { get; init; }
	public required Guid UserId { get; init; }
	public required Guid RecipeId { get; init; }
}
