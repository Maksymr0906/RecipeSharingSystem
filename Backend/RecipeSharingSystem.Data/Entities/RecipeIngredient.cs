using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class RecipeIngredient
{
	[Required]
	public Guid RecipeId { get; set; }

	[Required]
	public Guid IngredientId { get; set; }

	[Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
	public double Quantity { get; set; }

	[Required, MaxLength(50)]
	public string MeasurementUnit { get; set; } = string.Empty;

	public Recipe Recipe { get; set; } = null!;

	public Ingredient Ingredient { get; set; } = null!;
}
