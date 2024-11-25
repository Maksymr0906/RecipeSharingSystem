using FluentValidation;
using RecipeSharingSystem.Business.DTOs.Image;

namespace RecipeSharingSystem.Application.Validation.Image;

public class ImageDtoValidator : BaseValidator<ImageDto>
{
	public ImageDtoValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Image ID is required");

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

		RuleFor(x => x.Url)
			.Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
			.WithMessage("Image URL must be a valid absolute URL");

		RuleFor(x => x.DateCreated)
			.Must(date => date != default)
			.WithMessage("Date created must be a valid date");
	}
}