namespace RecipeSharingSystem.Core.Entities;

public class Category : BaseEntity
{
	public string Name { get; set; }
	public string FeaturedImageUrl { get; set; }
	public string Slug { get; set; }
	public ICollection<Recipe> Recipes { get; set; }
}
