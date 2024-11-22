namespace RecipeSharingSystem.Business.DTOs.User;

public record UpdateUserRequestDto(
	string? UserName,
	string? Email,
	string? FirstName,
	string? LastName,
	DateTime? DateOfBirth,
	string? PostalCode
);
