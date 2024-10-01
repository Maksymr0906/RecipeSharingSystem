namespace RecipeSharingSystem.Data.Entities
{
	public class Ingredient : BaseEntity
	{
		public string Name { get; set; }
		public ICollection<RecipeIngredient> RecipeIngredients { get; set;}
	}
}
