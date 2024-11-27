using FluentValidation;
using RecipeSharingSystem.Application.Validation;
using RecipeSharingSystem.Business.DTOs.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class ImageUploadModelValidator : BaseValidator<ImageUploadModel>
{
	public ImageUploadModelValidator()
	{
		RuleFor(x => x.FileExtension)
			.NotEmpty().WithMessage("File extension is required.")
			.Must(BeAnAllowedExtension).WithMessage("Unsupported file format.");

		RuleFor(x => x.FileContent)
			.NotNull().WithMessage("File content is missing.")
			.Must(BeValidFileSize).WithMessage("File size cannot be more than 10MB.")
			.Must(BeValidImage).WithMessage("Invalid image file.");
	}

	private bool BeAnAllowedExtension(string fileExtension)
	{
		if (string.IsNullOrEmpty(fileExtension)) return false;

		var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
		return allowedExtensions.Contains(fileExtension.ToLower());
	}

	private bool BeValidFileSize(byte[] fileContent)
	{
		if (fileContent == null) return false;
		return fileContent.Length > 0 && fileContent.Length <= 10485760; // 10MB
	}

	private bool BeValidImage(byte[] fileContent)
	{
		if (fileContent == null || fileContent.Length == 0) return false;

		try
		{
			using var memoryStream = new MemoryStream(fileContent);
			using var image = Image.Load<Rgba32>(memoryStream);

			return image.Width >= 960 && image.Height >= 960;
		}
		catch
		{
			return false;
		}
	}
}