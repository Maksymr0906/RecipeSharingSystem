using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Review;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
	: IReviewService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;

	public async Task<ReviewDto> CreateReviewAsync(CreateReviewRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var review = _mapper.Map<Review>(model);

		review = await _unitOfWork.ReviewRepository.CreateAsync(review);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ICollection<ReviewDto>> GetAllReviewsAsync()
	{
		var reviews = await _unitOfWork.ReviewRepository.GetAllWithDetails();

		return _mapper.Map<ICollection<ReviewDto>>(reviews);
	}

	public async Task<ReviewDto> GetReviewByIdAsync(Guid id)
	{
		var review = await _unitOfWork.ReviewRepository.GetByIdWithDetails(id);

		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ReviewDto> UpdateReviewAsync(Guid id, UpdateReviewRequestDto model)
	{
		await _validationService.ValidateAsync(model);

		var existingReview = await _unitOfWork.ReviewRepository.GetByIdWithDetails(id);

		_mapper.Map(model, existingReview);

		await _unitOfWork.ReviewRepository.UpdateAsync(existingReview);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<ReviewDto>(existingReview);
	}

	public async Task<ReviewDto> DeleteReviewAsync(Guid id)
	{
		var review = await _unitOfWork.ReviewRepository.DeleteByIdAsync(id);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ReviewDto> GetUserRecipeReview(Guid recipeId, Guid userId)
	{
		var review = await _unitOfWork.ReviewRepository.GetByUserAndRecipeId(userId, recipeId);

		await _unitOfWork.SaveAsync();

		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ICollection<ReviewDto>> GetRecipeReviews(Guid recipeId)
	{
		var reviews = await _unitOfWork.ReviewRepository.GetAllByRecipeId(recipeId);

		return _mapper.Map<ICollection<ReviewDto>>(reviews);
	}
}
