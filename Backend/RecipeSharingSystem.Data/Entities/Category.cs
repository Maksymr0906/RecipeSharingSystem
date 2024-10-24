namespace RecipeSharingSystem.Core.Entities;

public class Category : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public string FeaturedImageUrl { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public ICollection<Recipe> Recipes { get; set; } = [];
}
