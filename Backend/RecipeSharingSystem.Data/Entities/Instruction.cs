namespace RecipeSharingSystem.Data.Entities;

public class Instruction : BaseEntity
{
	public string Content { get; set; }
	public Recipe Recipe { get; set; }
}
