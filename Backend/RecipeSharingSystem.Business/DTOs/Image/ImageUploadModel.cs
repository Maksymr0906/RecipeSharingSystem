namespace RecipeSharingSystem.Business.DTOs.Image;

public record ImageUploadModel(
	string FileName,
	string FileExtension,
	string Title,
	string LocalPath,
	string UrlPath,
	byte[] FileContent
);
