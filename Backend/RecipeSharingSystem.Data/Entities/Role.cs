﻿namespace RecipeSharingSystem.Core.Entities;

public class Role
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public ICollection<RolePermission> RolePermissions { get; set; }

	public ICollection<UserRole> UserRoles { get; set; }
}
