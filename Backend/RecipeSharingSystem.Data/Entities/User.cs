using System.ComponentModel.DataAnnotations;

namespace RecipeSharingSystem.Core.Entities;

public class User : BaseEntity
{
	[Required, MaxLength(100)]
	public string UserName { get; set; } = string.Empty;

	[Required, EmailAddress, MaxLength(255)]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string PasswordHash { get; set; } = string.Empty;

	[MaxLength(50)]
	public string? FirstName { get; set; }

	[MaxLength(50)]
	public string? LastName { get; set; }

	public DateTime? DateOfBirth { get; set; }

	[MaxLength(20)]
	public string? PostalCode { get; set; }

	public ICollection<Recipe> AuthoredRecipes { get; set; } =  new List<Recipe>();

	public ICollection<UserFavoriteRecipe> FavoriteRecipes { get; set; } = new List<UserFavoriteRecipe>();

	public ICollection<Review> Reviews { get; set; } = new List<Review>();

	public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
