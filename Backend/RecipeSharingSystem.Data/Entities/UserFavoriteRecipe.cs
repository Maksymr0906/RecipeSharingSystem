using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class UserFavoriteRecipe
{
	[Required]
	public Guid RecipeId { get; set; }

	[Required]
	public Guid UserId { get; set; }

	public Recipe Recipe { get; set; } = null!;

	public User User { get; set; } = null!;
}
