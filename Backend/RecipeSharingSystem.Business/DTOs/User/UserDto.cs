namespace RecipeSharingSystem.Business.DTOs.User;

public record UserDto(
	Guid Id,
	string UserName,
	string Email,
	string PasswordHash,
	bool IsAdmin,
	IEnumerable<Guid> CommentIds,
	IEnumerable<Guid> RatingIds
);
