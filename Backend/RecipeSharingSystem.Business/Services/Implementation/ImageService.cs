using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Data.Entities;

namespace RecipeSharingSystem.Business.Services.Implementation
{
    public class ImageService : IImageService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ImageService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ImageDto> CreateImageAsync(ImageUploadModel model)
		{
			ValidateFileUpload(model);
			using var stream = new FileStream(model.LocalPath, FileMode.Create, FileAccess.Write);
			using var memoryStream = new MemoryStream(model.FileContent);
			await memoryStream.CopyToAsync(stream);
			var image = _mapper.Map<Image>(model);
			image = await _unitOfWork.ImageRepository.CreateAsync(image);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<ImageDto>(image);
		}

		public async Task<ICollection<ImageDto>> GetAllImagesAsync()
		{
			var images = await _unitOfWork.ImageRepository.GetAllAsync();
			return _mapper.Map<ICollection<ImageDto>>(images);
		}

		public async Task<ImageDto> GetImageByIdAsync(Guid id)
		{
			var image = await _unitOfWork.ImageRepository.GetByIdAsync(id);
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
