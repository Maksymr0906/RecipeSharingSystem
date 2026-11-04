using FluentValidation;
using RecipeSharingSystem.Application.DTOs.Auth;

namespace RecipeSharingSystem.Application.Validation.Auth;

public class LoginRequestDtoValidator : BaseValidator<LoginRequestDto>
{
	public LoginRequestDtoValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid email format.")
			.MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

		// No minimum length here: the registration policy must not be re-applied at login,
		// otherwise accounts created under an older, weaker policy could no longer sign in.
		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MaximumLength(128).WithMessage("Password must not exceed 128 characters.");
	}
}
