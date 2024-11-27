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

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(5).WithMessage("Password must be at least 5 characters long.")
			.MaximumLength(50).WithMessage("Password must not exceed 50 characters.");
	}
}
