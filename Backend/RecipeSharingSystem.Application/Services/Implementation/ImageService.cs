using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Image;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;
using SixLabors.ImageSharp.PixelFormats;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class ImageService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
	: IImageService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;

	public async Task<ImageDto> CreateImageAsync(ImageUploadModel model)
	{
		await _validationService.ValidateAsync(model);

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
}
