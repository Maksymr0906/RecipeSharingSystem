namespace RecipeSharingSystem.Core.Entities;

public class RecipeIngredient : BaseEntity
{
	public Guid RecipeId { get; set; }
	public Guid IngredientId { get; set; }
	public double Quantity { get; set; }
	public string MeasurementUnit { get; set; } = string.Empty;
	public Recipe Recipe { get; set; }
	public Ingredient Ingredient { get; set; }
}
