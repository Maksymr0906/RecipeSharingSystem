namespace RecipeSharingSystem.Business.DTOs.Recipe;

public class RecipeDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string ShortDescription { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public string FeaturedImageUrl { get; set; } = string.Empty;
	public Guid InstructionId { get; set; }
	public Guid AuthorId { get; set; }
	public string AuthorUserName {  get; set; } = string.Empty;
	public float Rating { get; set; }
	public ICollection<Guid> CategoryIds { get; set; } = [];
	public ICollection<IngredientQuantityDto> Ingredients { get; set; } = [];
}
