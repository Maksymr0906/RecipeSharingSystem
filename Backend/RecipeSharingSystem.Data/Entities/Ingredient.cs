using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class Ingredient : BaseEntity
{
	[Required, MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	[MaxLength(100)]
	public string Slug { get; set; } = string.Empty;

	public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
