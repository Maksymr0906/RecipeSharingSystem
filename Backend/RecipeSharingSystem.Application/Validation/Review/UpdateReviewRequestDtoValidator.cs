using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Review;

namespace RecipeSharingSystem.Application.Validation.Review;

public class UpdateReviewRequestDtoValidator : BaseValidator<UpdateReviewRequestDto>
{
	public UpdateReviewRequestDtoValidator()
	{
		RuleFor(x => x.Rating)
			.InclusiveBetween(1, 5)
			.WithMessage("Rating must be between 1 and 5");

		RuleFor(x => x.Content)
			.MaximumLength(2000)
			.When(x => !string.IsNullOrEmpty(x.Content))
			.WithMessage("Review content must be less than 2000 characters");
	}
}