using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class IngredientService : IIngredientService
	{
		private readonly IIngredientRepository _repository;
		private readonly IMapper _mapper;

		public IngredientService(IIngredientRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequestDto model)
		{
			var ingredient = _mapper.Map<Ingredient>(model);
			ingredient = await _repository.CreateAsync(ingredient);
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<ICollection<IngredientDto>> GetAllIngredientsAsync()
		{
			var ingredients = await _repository.GetAllAsync();
			return _mapper.Map<ICollection<IngredientDto>>(ingredients);
		}

		public async Task<IngredientDto> GetIngredientByIdAsync(Guid id)
		{
			var ingredient = await _repository.GetByIdAsync(id);
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<IngredientDto> UpdateIngredientAsync(Guid id, UpdateIngredientRequestDto model)
		{
			var ingredient = _mapper.Map<Ingredient>(model);
			ingredient.Id = id;
			ingredient = await _repository.UpdateAsync(ingredient);
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<IngredientDto> DeleteIngredientAsync(Guid id)
		{
			var ingredient = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<IngredientDto>(ingredient);
		}
	}
}
