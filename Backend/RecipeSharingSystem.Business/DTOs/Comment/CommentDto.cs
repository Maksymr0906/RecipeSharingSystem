namespace RecipeSharingSystem.Business.DTOs.Comment
{
	public class CommentDto
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public DateTime DateCreated { get; set; }
		public Guid UserId { get; set; }
		public Guid RecipeId { get; set; }
	}
}
