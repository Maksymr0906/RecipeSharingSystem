namespace RecipeSharingSystem.Core.Entities;

public class Review : BaseEntity
{
	public int Rating { get; set; }
	public string? Content { get; set; }
	public DateTime DateCreated { get; set; }
	public Guid UserId { get; set; }
	public Guid RecipeId { get; set; }
	public User User { get; set; }
	public Recipe Recipe { get; set; }
}