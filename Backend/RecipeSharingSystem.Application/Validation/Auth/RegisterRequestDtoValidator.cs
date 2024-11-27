using FluentValidation;
using RecipeSharingSystem.Application.DTOs.Auth;

namespace RecipeSharingSystem.Application.Validation.Auth;

public class RegisterRequestDtoValidator : BaseValidator<RegisterRequestDto>
{
	public RegisterRequestDtoValidator()
	{
		RuleFor(x => x.UserName)
			.NotEmpty().WithMessage("Username is required.")
			.MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
			.MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
			.Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid email format.")
			.MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(5).WithMessage("Password must be at least 5 characters long.")
			.MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
	}
}
