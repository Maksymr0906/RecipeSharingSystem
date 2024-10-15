namespace RecipeSharingSystem.Business.DTOs.Recipe;

public record IngredientQuantityDto(
	string IngredientName,
	double Quantity,
	string MeasurementUnit
);
