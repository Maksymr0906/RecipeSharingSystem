namespace RecipeSharingSystem.Business.DTOs.User;

public class UserDto
{
	public Guid Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? FirstName {  get; set; }
	public string? LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? PostalCode { get; set; }
	public ICollection<Guid> AuthoredRecipeIds { get; set; } = [];
	public ICollection<Guid> FavoriteRecipeIds { get; set; } = [];
	public ICollection<Guid> ReviewIds { get; set; } = [];
}
