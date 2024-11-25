using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class Review : BaseEntity
{
	[Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
	public int Rating { get; set; }

	[MaxLength(2000)]
	public string? Content { get; set; }

	[Required]
	public DateTime DateCreated { get; set; }

	[Required]
	public Guid UserId { get; set; }

	[Required]
	public Guid RecipeId { get; set; }

	public User User { get; set; } = null!;

	public Recipe Recipe { get; set; } = null!;
}