namespace RecipeSharingSystem.Business.DTOs.User;

public class UserDto
{
	public Guid Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public bool IsAdmin { get; set; }
	public IEnumerable<Guid> CommentIds { get; set; } = [];
	public IEnumerable<Guid> RatingIds { get; set; } = [];
}
