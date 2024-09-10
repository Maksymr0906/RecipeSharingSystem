namespace RecipeSharingSystem.Data.Entities
{
	public class Rating : BaseEntity
	{
		public int Value { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
		public Guid RecipeId { get; set; }
		public Recipe Recipe { get; set; }
	}
}
