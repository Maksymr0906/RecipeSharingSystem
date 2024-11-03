namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record CategorySeedData
{
	public required Guid Id { get; init; }
	public required string Name { get; init; }
	public required string FeaturedImageUrl { get; init; }
	public required string Slug { get; init; }
}