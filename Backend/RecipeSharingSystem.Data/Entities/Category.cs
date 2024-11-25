using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class Category : BaseEntity
{
	[Required]
	[MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
	public string Name { get; set; } = string.Empty;

	[Required]
	[Url(ErrorMessage = "Please provide a valid URL.")]
	public string FeaturedImageUrl { get; set; } = string.Empty;

	[Required]
	[MaxLength(100, ErrorMessage = "Slug cannot exceed 100 characters.")]
	public string Slug { get; set; } = string.Empty;

	public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
