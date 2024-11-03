using AutoMapper;
using RecipeSharingSystem.Application.DTOs.Reviews;
using RecipeSharingSystem.Business.DTOs.Review;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
	: IReviewService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;

	public async Task<ReviewDto> CreateReviewAsync(CreateReviewRequestDto model)
	{
		var review = _mapper.Map<Review>(model);
		review = await _unitOfWork.ReviewRepository.CreateAsync(review);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ICollection<ReviewDto>> GetAllReviewsAsync()
	{
		var Reviews = await _unitOfWork.ReviewRepository.GetAllAsync();
		return _mapper.Map<ICollection<ReviewDto>>(Reviews);
	}

	public async Task<ReviewDto> GetReviewByIdAsync(Guid id)
	{
		var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ReviewDto> UpdateReviewAsync(Guid id, UpdateReviewRequestDto model)
	{
		var review = _mapper.Map<Review>(model);
		review.Id = id;
		review = await _unitOfWork.ReviewRepository.UpdateAsync(review);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ReviewDto> DeleteReviewAsync(Guid id)
	{
		var review = await _unitOfWork.ReviewRepository.DeleteByIdAsync(id);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<ReviewDto>(review);
	}

	public async Task<ReviewDto> GetUserRecipeReview(UserRecipeReviewRequestDto model)
	{
		var review = await _unitOfWork.ReviewRepository.GetByUserAndRecipeId(model.userId, model.recipeId);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<ReviewDto>(review);
	}
}
