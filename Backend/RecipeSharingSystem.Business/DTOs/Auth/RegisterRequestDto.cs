namespace RecipeSharingSystem.Application.DTOs.Auth;

public record RegisterRequestDto(
	string UserName,
	string Email,
	string Password
);