namespace RecipeSharingSystem.Core.Entities;

public class Recipe : BaseEntity
{
	public string Title { get; set; } = string.Empty;
	public string ShortDescription { get; set; } = string.Empty;
	public string FeaturedImageUrl { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public Guid InstructionId { get; set; }
	public Guid AuthorId { get; set; }
	public Instruction Instruction { get; set; }
	public User Author { get; set; }
	public ICollection<UserFavoriteRecipe> FavoriteBy { get; set; } = [];
	public ICollection<Review> Reviews { get; set; } = [];
	public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
	public ICollection<Category> Categories { get; set; } = [];
}
