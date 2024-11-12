namespace RecipeSharingSystem.Core.Entities;

public class User : BaseEntity
{
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public ICollection<Review> Reviews { get; set; } = [];
	public ICollection<UserRole> UserRoles { get; set; } = [];
}
