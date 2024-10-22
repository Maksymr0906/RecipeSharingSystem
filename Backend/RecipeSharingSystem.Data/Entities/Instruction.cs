namespace RecipeSharingSystem.Core.Entities;

public class Instruction : BaseEntity
{
	public string Content { get; set; }
	public Recipe Recipe { get; set; }
}
