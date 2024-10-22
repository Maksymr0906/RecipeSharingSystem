namespace RecipeSharingSystem.Core.Entities;

public class Ingredient : BaseEntity
{
	public string Name { get; set; }
	public string Slug { get; set; }
	public ICollection<RecipeIngredient> RecipeIngredients { get; set;}
}
