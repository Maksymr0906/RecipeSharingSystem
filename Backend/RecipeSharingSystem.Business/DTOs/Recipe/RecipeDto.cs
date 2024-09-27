namespace RecipeSharingSystem.Business.DTOs.Recipe
{
	public class RecipeDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public Guid InstructionId { get; set; }
		public Guid ImageId { get; set; }
		public IEnumerable<Guid> RatingIds { get; set; }
		public IEnumerable<Guid> CategoryIds { get; set; }
		public ICollection<IngredientQuantityDto> Ingredients { get; set; }
	}
}
