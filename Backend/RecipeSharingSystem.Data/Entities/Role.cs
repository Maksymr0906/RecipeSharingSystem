namespace RecipeSharingSystem.Core.Entities;

public class Role
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

	public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
