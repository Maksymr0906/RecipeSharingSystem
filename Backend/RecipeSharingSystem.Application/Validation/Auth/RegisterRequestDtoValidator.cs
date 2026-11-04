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

		// Length is the main strength factor (NIST SP 800-63B); the upper bound only limits abuse.
		// Passwords are hashed with BCrypt "enhanced" mode (SHA-384 pre-hash), so the 72-byte bcrypt limit does not apply.
		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.MaximumLength(128).WithMessage("Password must not exceed 128 characters.")
			.Must((model, password) => !string.Equals(password, model.UserName, StringComparison.OrdinalIgnoreCase)
				&& !string.Equals(password, model.Email, StringComparison.OrdinalIgnoreCase))
			.WithMessage("Password must not be the same as the username or email.");
	}
}
