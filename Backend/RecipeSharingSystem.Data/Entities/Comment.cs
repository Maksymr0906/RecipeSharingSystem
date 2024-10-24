namespace RecipeSharingSystem.Core.Entities;

public class Comment : BaseEntity
{
	public string Content { get; set; } = string.Empty;
	public DateTime DateCreated { get; set; }
	public Guid UserId { get; set; }
	public Guid RecipeId { get; set; }
	public User User { get; set; }
	public Recipe Recipe { get; set; }
}
