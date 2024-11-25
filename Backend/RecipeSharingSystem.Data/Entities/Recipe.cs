using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class Recipe : BaseEntity
{
	[Required, MaxLength(255)]
	public string Title { get; set; } = string.Empty;

	[MaxLength(1000)]
	public string ShortDescription { get; set; } = string.Empty;

	[MaxLength(255), Url]
	public string FeaturedImageUrl { get; set; } = string.Empty;

	[Required, MaxLength(100)]
	public string Slug { get; set; } = string.Empty;

	[Required]
	public Guid InstructionId { get; set; }

	[Required]
	public Guid AuthorId { get; set; }

	public Instruction Instruction { get; set; } = null!;

	public User Author { get; set; } = null!;

	public ICollection<UserFavoriteRecipe> FavoriteBy { get; set; } = new List<UserFavoriteRecipe>();

	public ICollection<Review> Reviews { get; set; } = new List<Review>();

	public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

	public ICollection<Category> Categories { get; set; } = new List<Category>();
}
