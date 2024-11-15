namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record RecipeSeedData
{
	public required Guid Id { get; init; }
	public required string Title { get; init; }
	public required string ShortDescription { get; init; }
	public required string FeaturedImageUrl { get; init; }
	public required string Slug { get; init; }
	public required Guid InstructionId { get; init; }
	public required Guid AuthorId { get; init; }
}
