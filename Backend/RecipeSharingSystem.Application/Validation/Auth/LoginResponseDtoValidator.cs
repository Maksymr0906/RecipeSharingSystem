using FluentValidation;
using RecipeSharingSystem.Application.DTOs.Auth;

namespace RecipeSharingSystem.Application.Validation.Auth;

public class LoginResponseDtoValidator : BaseValidator<LoginResponseDto>
{
	public LoginResponseDtoValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.WithMessage("User ID is required");

		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress()
			.WithMessage("Valid email is required");

		RuleFor(x => x.Token)
			.NotEmpty()
			.WithMessage("Authentication token is required");

		RuleFor(x => x.Roles)
			.NotNull()
			.Must(roles => roles.Any())
			.WithMessage("At least one role is required");
	}
}