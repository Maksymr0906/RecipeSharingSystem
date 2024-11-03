namespace RecipeSharingSystem.Business.DTOs.User;

public class UserDto
{
	public Guid Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public bool IsAdmin { get; set; }
	public ICollection<Guid> ReviewIds { get; set; } = [];
	public ICollection<Guid> RatingIds { get; set; } = [];
}
