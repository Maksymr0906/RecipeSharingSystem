using RecipeSharingSystem.Business.DTOs.Review;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IReviewService
{
	Task<ReviewDto> CreateReviewAsync(CreateReviewRequestDto model);
	Task<ICollection<ReviewDto>> GetAllReviewsAsync();
	Task<ReviewDto> GetReviewByIdAsync(Guid id);
	Task<ReviewDto> UpdateReviewAsync(Guid id, UpdateReviewRequestDto model);
	Task<ReviewDto> DeleteReviewAsync(Guid id);
	Task<ReviewDto> GetUserRecipeReview(Guid recipeId, Guid userId);
	Task<ICollection<ReviewDto>> GetRecipeReviews(Guid recipeId);
}
