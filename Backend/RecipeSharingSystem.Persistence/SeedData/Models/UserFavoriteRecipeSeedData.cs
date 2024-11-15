namespace RecipeSharingSystem.Persistence.SeedData.Models;

public record UserFavoriteRecipeSeedData
{
	public required Guid RecipeId { get; init; }
	public required Guid UserId { get; init; }
}
