using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Persistence;

public class SeedDataOptions
{
	public User[] Users { get; set; } = [];
	public UserRole[] UserRoles { get; set; } = [];
}
