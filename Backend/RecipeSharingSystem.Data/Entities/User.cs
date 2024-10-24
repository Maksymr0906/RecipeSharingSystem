namespace RecipeSharingSystem.Core.Entities;

public class User : BaseEntity
{
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public ICollection<Comment> Comments { get; set; } = [];
	public ICollection<Rating> Ratings { get; set; } = [];
	public ICollection<UserRole> UserRoles { get; set; } = [];
}
