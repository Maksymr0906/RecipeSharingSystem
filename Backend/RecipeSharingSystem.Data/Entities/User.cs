﻿namespace RecipeSharingSystem.Core.Entities;

public class User : BaseEntity
{
	public string UserName { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public ICollection<Comment> Comments { get; set; }
	public ICollection<Rating> Ratings { get; set; }
	public ICollection<UserRole> UserRoles { get; set; }
}
