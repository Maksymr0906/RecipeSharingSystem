namespace RecipeSharingSystem.Core.Entities;

public class Recipe : BaseEntity
{
	public string Title { get; set; }
	public string ShortDescription { get; set; }
	public string FeaturedImageUrl { get; set; }
	public string Slug { get; set; }
	public Guid InstructionId { get; set; }
	public Instruction Instruction { get; set; }
	public ICollection<Rating> Ratings { get; set; }
	public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
	public ICollection<Category> Categories { get; set; }
}
