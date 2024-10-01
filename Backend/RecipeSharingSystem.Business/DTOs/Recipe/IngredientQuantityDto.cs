namespace RecipeSharingSystem.Business.DTOs.Recipe
{
	public class IngredientQuantityDto
	{
		public Guid IngredientId { get; set; }
		public double Quantity { get; set; }
		public string MeasurementUnit { get; set; }
	}
}
