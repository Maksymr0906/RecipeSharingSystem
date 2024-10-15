namespace RecipeSharingSystem.Business.DTOs.User;

public record CreateUserRequestDto(
	string UserName,
	string Email,
	string PasswordHash,
	bool IsAdmin
);
