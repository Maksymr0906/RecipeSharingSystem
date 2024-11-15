namespace RecipeSharingSystem.Core.Entities;

public class UserFavoriteRecipe
{
	public Guid RecipeId { get; set; }
	public Guid UserId { get; set; }
	public Recipe Recipe { get; set; }
	public User User { get; set; }
}
