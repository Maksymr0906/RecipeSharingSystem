namespace RecipeSharingSystem.Data.Entities
{
	public class Recipe : BaseEntity
	{
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public Guid InstructionId { get; set; }
		public Instruction Instruction { get; set; }
		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
		public Guid ImageId { get; set; }
		public Image Image { get; set; }
		public ICollection<Rating> Ratings { get; set; }
		public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
	}
}
