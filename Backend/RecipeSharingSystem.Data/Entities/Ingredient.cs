namespace RecipeSharingSystem.Core.Entities;

public class Ingredient : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
}
