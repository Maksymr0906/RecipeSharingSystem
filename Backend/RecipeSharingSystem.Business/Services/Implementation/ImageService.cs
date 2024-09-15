using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class ImageService : IImageService
	{
		private readonly IImageRepository _repository;
		private readonly IMapper _mapper;

		public ImageService(IImageRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<ImageDto> CreateImageAsync(ImageUploadModel model)
		{
			ValidateFileUpload(model);
			using var stream = new FileStream(model.LocalPath, FileMode.Create, FileAccess.Write);
			using var memoryStream = new MemoryStream(model.FileContent);
			await memoryStream.CopyToAsync(stream);
			var image = _mapper.Map<Image>(model);
			image = await _repository.CreateAsync(image);
			return _mapper.Map<ImageDto>(image);
		}

		public async Task<IEnumerable<ImageDto>> GetAllImagesAsync()
		{
			var images = await _repository.GetAllAsync();
			return _mapper.Map<IEnumerable<ImageDto>>(images);
		}

		public async Task<ImageDto> GetImageByIdAsync(Guid id)
		{
			var image = await _repository.GetByIdAsync(id);
			return _mapper.Map<ImageDto>(image);
		}

		private void ValidateFileUpload(ImageUploadModel model)
		{
			var allowedExtensions = new List<string>() { ".jpg", ".jpeg", ".png" };
			if (!allowedExtensions.Contains(model.FileExtension.ToLower()))
			{
				throw new ArgumentException("Unsupported file format.");
			}

			if (model.FileContent == null || model.FileContent.Length == 0)
			{
				throw new ArgumentException("File content is missing.");
			}

			if (model.FileContent.Length > 10485760)
			{
				throw new ArgumentException("File size cannot be more than 10MB.");
			}
		}
	}
}
