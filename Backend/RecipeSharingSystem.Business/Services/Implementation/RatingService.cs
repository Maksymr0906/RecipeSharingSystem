using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Rating;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class RatingService : IRatingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<RatingDto> CreateRatingAsync(CreateRatingRequestDto model)
		{
			var rating = _mapper.Map<Rating>(model);
			rating = await _unitOfWork.RatingRepository.CreateAsync(rating);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<ICollection<RatingDto>> GetAllRatingAsync()
		{
			var ratings = await _unitOfWork.RatingRepository.GetAllAsync();
			return _mapper.Map<ICollection<RatingDto>>(ratings);
		}

		public async Task<RatingDto> GetRatingByIdAsync(Guid id)
		{
			var rating = await _unitOfWork.RatingRepository.GetByIdAsync(id);
			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<RatingDto> UpdateRatingAsync(Guid id, UpdateRatingRequestDto model)
		{
			var rating = _mapper.Map<Rating>(model);
			rating.Id = id;
			rating = await _unitOfWork.RatingRepository.UpdateAsync(rating);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<RatingDto> DeleteRatingAsync(Guid id)
		{
			var rating = await _unitOfWork.RatingRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<RatingDto>(rating);
		}
	}
}
