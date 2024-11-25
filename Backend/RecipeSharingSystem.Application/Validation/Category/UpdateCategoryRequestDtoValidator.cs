using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Category;

namespace RecipeSharingSystem.Application.Validation.Category;

public class UpdateCategoryRequestDtoValidator : BaseValidator<UpdateCategoryRequestDto>
{
	public UpdateCategoryRequestDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Category name is required.")
			.MinimumLength(2).WithMessage("Category name must be at least 2 characters long.")
			.MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

		RuleFor(x => x.Slug)
			.NotEmpty().WithMessage("Slug is required.")
			.MinimumLength(2).WithMessage("Slug must be at least 2 characters long.")
			.MaximumLength(100).WithMessage("Slug must not exceed 100 characters.")
			.Matches(@"^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens.");

		RuleFor(x => x.FeaturedImageUrl)
			.NotEmpty().WithMessage("Featured image URL is required.")
			.Must(BeAValidUrl).WithMessage("Invalid URL format.");
	}

	private bool BeAValidUrl(string url)
	{
		return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
			   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}
}
