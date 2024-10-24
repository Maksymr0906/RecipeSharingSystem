namespace RecipeSharingSystem.Core.Entities;

public class Instruction : BaseEntity
{
	public string Content { get; set; } = string.Empty;
	public Recipe Recipe { get; set; }
}
