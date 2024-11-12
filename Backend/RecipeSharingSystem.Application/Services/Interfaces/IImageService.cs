using RecipeSharingSystem.Business.DTOs.Image;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IImageService
{
	Task<ImageDto> CreateImageAsync(ImageUploadModel model);
	Task<ICollection<ImageDto>> GetAllImagesAsync();
	Task<ImageDto> GetImageByIdAsync(Guid id);
}
