namespace RecipeSharingSystem.Business.DTOs.Recipe
{
	public class CreateRecipeRequestDto
	{
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public Guid InstructionId { get; set; }
		public Guid CategoryId { get; set; }
		public Guid ImageId { get; set; }
		public IEnumerable<IngredientQuantityDto> Ingredients { get; set; }
	}
}
