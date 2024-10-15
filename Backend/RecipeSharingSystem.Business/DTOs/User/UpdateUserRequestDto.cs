namespace RecipeSharingSystem.Business.DTOs.User;

public record UpdateUserRequestDto(
	string UserName,
	string Email,
	string PasswordHash,
	bool IsAdmin
);
