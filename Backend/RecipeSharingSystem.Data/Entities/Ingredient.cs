namespace RecipeSharingSystem.Data.Entities
{
	public class Ingredient : BaseEntity
	{
		public string Name { get; set; }
		public Guid ImageId { get; set; }
		public Image Image { get; set; }
		public ICollection<RecipeIngredient> RecipeIngredients { get; set;}
	}
}
