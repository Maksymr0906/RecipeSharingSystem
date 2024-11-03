using RecipeSharingSystem.Business.DTOs.Rating;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface IRatingService
{
	Task<RatingDto> CreateRatingAsync(CreateRatingRequestDto model);
	Task<ICollection<RatingDto>> GetAllRatingAsync();
	Task<RatingDto> GetRatingByIdAsync(Guid id);
	Task<RatingDto> UpdateRatingAsync(Guid id, UpdateRatingRequestDto model);
	Task<RatingDto> DeleteRatingAsync(Guid id);
	Task<RatingDto> GetUserRecipeRatingAsync(UserRecipeRatingRequestDto model);
}
