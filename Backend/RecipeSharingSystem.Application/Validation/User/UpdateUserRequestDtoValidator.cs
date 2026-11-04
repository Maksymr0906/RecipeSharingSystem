using FluentValidation;
using RecipeSharingSystem.Business.DTOs.User;

namespace RecipeSharingSystem.Application.Validation.User;

public class UpdateUserRequestDtoValidator : BaseValidator<UpdateUserRequestDto>
{
	private static readonly DateTime MinimumDateOfBirth = new(1900, 1, 1);

	public UpdateUserRequestDtoValidator()
	{
		// Every field is optional (null = "leave unchanged"), but a supplied value must satisfy the same limits as on registration.
		RuleFor(x => x.UserName)
			.MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
			.MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
			.Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.")
			.When(x => x.UserName != null);

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email must not be empty.")
			.EmailAddress().WithMessage("Invalid email format.")
			.MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
			.When(x => x.Email != null);

		RuleFor(x => x.FirstName)
			.MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
			.When(x => x.FirstName != null);

		RuleFor(x => x.LastName)
			.MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
			.When(x => x.LastName != null);

		RuleFor(x => x.PostalCode)
			.MaximumLength(20).WithMessage("Postal code must not exceed 20 characters.")
			.When(x => x.PostalCode != null);

		RuleFor(x => x.DateOfBirth)
			.Must(date => date!.Value >= MinimumDateOfBirth && date.Value.Date <= DateTime.UtcNow.Date.AddDays(1))
			.WithMessage("Date of birth must be between 1900-01-01 and today.")
			.When(x => x.DateOfBirth.HasValue);
	}
}
