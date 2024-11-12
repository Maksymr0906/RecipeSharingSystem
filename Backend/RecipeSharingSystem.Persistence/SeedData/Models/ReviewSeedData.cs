namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record ReviewSeedData
{
	public required Guid Id { get; init; }
	public required int Rating { get; init; }
	public string? Content { get; init; }
	public required DateTime DateCreated { get; init; }
	public required Guid UserId { get; init; }
	public required Guid RecipeId { get; init; }
}
