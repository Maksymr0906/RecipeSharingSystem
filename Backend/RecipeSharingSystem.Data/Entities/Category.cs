namespace RecipeSharingSystem.Data.Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
		public string FeaturedImageUrl { get; set; }
		public ICollection<Recipe> Recipes { get; set; }
	}
}
