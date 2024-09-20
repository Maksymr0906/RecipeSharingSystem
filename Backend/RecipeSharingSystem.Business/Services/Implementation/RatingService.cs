using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Rating;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class RatingService : IRatingService
	{
		private readonly IRatingRepository _repository;
		private readonly IMapper _mapper;

		public RatingService(IRatingRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<RatingDto> CreateRatingAsync(CreateRatingRequestDto model)
		{
			var rating = _mapper.Map<Rating>(model);
			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<ICollection<RatingDto>> GetAllRatingAsync()
		{
			var ratings = await _repository.GetAllAsync();
			return _mapper.Map<ICollection<RatingDto>>(ratings);
		}

		public async Task<RatingDto> GetRatingByIdAsync(Guid id)
		{
			var rating = await _repository.GetByIdAsync(id);
			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<RatingDto> UpdateRatingAsync(Guid id, UpdateRatingRequestDto model)
		{
			var rating = _mapper.Map<Rating>(model);
			rating.Id = id;
			rating = await _repository.UpdateAsync(rating);
			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<RatingDto> DeleteRatingAsync(Guid id)
		{
			var rating = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<RatingDto>(rating);
		}
	}
}
