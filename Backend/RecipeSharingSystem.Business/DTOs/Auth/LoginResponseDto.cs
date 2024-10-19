namespace RecipeSharingSystem.Application.DTOs.Auth;

public record LoginResponseDto(
	string UserId,
	string Email,
	string Token,
	List<string> Roles
);