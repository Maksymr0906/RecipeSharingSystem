namespace RecipeSharingSystem.Business.DTOs.Recipe;

public class RecipeDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string ShortDescription { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string FeaturedImageUrl { get; set; } = string.Empty;
	public Guid InstructionId { get; set; }
	public IEnumerable<Guid> RatingIds { get; set; } = [];
	public IEnumerable<Guid> CategoryIds { get; set; } = [];
	public ICollection<IngredientQuantityDto> Ingredients { get; set; } = [];
}
