using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Image;

namespace RecipeSharingSystem.Application.Validation.Image;

public class ImageUploadModelValidator : BaseValidator<ImageUploadModel>
{
	public ImageUploadModelValidator()
	{
		RuleFor(x => x.FileName)
			.NotEmpty()
			.MaximumLength(255)
			.WithMessage("File name is required and must be less than 255 characters");

		RuleFor(x => x.FileExtension)
			.NotEmpty()
			.Matches(@"^\.[a-zA-Z0-9]+$")
			.WithMessage("File extension must start with a dot and contain only alphanumeric characters");

		RuleFor(x => x.Title)
			.MaximumLength(200)
			.WithMessage("Title must be less than 200 characters");

		RuleFor(x => x.LocalPath)
			.NotEmpty()
			.WithMessage("Local path is required");

		RuleFor(x => x.UrlPath)
			.Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
			.WithMessage("URL path must be a valid absolute URL");

		RuleFor(x => x.FileContent)
			.NotNull()
			.Must(content => content != null && content.Length > 0)
			.WithMessage("File content is required");
	}
}