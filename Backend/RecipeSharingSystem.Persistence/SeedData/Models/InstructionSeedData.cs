namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record InstructionSeedData
{
	public required Guid Id { get; init; }
	public required string Content { get; init; }
}
