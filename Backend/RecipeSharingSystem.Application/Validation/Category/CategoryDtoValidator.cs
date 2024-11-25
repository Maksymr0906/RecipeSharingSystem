using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Category;

namespace RecipeSharingSystem.Application.Validation.Category;

public class CategoryDtoValidator : BaseValidator<CategoryDto>
{
	public CategoryDtoValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Category ID is required");

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100)
			.WithMessage("Category name is required and must be less than 100 characters");

		RuleFor(x => x.Slug)
			.NotEmpty()
			.MaximumLength(150)
			.Matches("^[a-z0-9-]+$")
			.WithMessage("Slug must be lowercase, alphanumeric, and can contain hyphens");

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