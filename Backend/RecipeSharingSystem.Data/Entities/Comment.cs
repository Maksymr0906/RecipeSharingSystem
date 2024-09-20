namespace RecipeSharingSystem.Data.Entities
{
	public class Comment : BaseEntity
	{
		public string Content { get; set; }
		public DateTime DateCreated { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
		public Guid RecipeId { get; set; }
		public Recipe Recipe { get; set; }
	}
}
