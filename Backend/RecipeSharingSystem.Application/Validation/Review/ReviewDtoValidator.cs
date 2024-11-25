using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Review;

namespace RecipeSharingSystem.Application.Validation.Review;

public class ReviewDtoValidator : BaseValidator<ReviewDto>
{
	public ReviewDtoValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Review ID is required");

		RuleFor(x => x.Rating)
			.InclusiveBetween(1, 5)
			.WithMessage("Rating must be between 1 and 5");

		RuleFor(x => x.Content)
			.MaximumLength(2000)
			.When(x => !string.IsNullOrEmpty(x.Content))
			.WithMessage("Review content must be less than 2000 characters");

		RuleFor(x => x.DateCreated)
			.Must(date => date != default)
			.WithMessage("Date created must be a valid date");

		RuleFor(x => x.UserName)
			.NotEmpty()
			.MaximumLength(100)
			.WithMessage("User name is required and must be less than 100 characters");

		RuleFor(x => x.UserId)
			.NotEmpty()
			.WithMessage("User ID is required");

		RuleFor(x => x.RecipeId)
			.NotEmpty()
			.WithMessage("Recipe ID is required");
	}
}