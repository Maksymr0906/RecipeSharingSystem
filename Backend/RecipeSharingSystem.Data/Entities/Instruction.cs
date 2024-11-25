using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class Instruction : BaseEntity
{
	[Required]
	public string Content { get; set; } = string.Empty;

	public Recipe Recipe { get; set; } = null!;
}
