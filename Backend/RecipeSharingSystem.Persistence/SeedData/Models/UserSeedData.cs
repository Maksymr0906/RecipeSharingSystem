namespace RecipeSharingSystem.Persistence.SeedData.Models;
public record UserSeedData
{
	public required Guid Id { get; init; }
	public required string UserName { get; init; }
	public required string Email { get; init; }
	public required string PasswordHash { get; init; }
}
