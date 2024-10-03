using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class IngredientService : IIngredientService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public IngredientService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequestDto model)
		{
			var ingredient = _mapper.Map<Ingredient>(model);
			ingredient = await _unitOfWork.IngredientRepository.CreateAsync(ingredient);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<ICollection<IngredientDto>> GetAllIngredientsAsync()
		{
			var ingredients = await _unitOfWork.IngredientRepository.GetAllAsync();
			return _mapper.Map<ICollection<IngredientDto>>(ingredients);
		}

		public async Task<IngredientDto> GetIngredientByIdAsync(Guid id)
		{
			var ingredient = await _unitOfWork.IngredientRepository.GetByIdAsync(id);
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<IngredientDto> UpdateIngredientAsync(Guid id, UpdateIngredientRequestDto model)
		{
			var ingredient = _mapper.Map<Ingredient>(model);
			ingredient.Id = id;
			ingredient = await _unitOfWork.IngredientRepository.UpdateAsync(ingredient);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<IngredientDto> DeleteIngredientAsync(Guid id)
		{
			var ingredient = await _unitOfWork.IngredientRepository.DeleteByIdAsync(id);
			await _unitOfWork.SaveAsync();
			return _mapper.Map<IngredientDto>(ingredient);
		}

		public async Task<Ingredient> GetOrCreateIngredientAsync(string ingredientName)
		{
			var existingIngredient = await _unitOfWork.IngredientRepository.GetByNameAsync(ingredientName);
			if (existingIngredient != null)
			{
				return existingIngredient;
			}

			var newIngredient = new Ingredient { Name = ingredientName };
			newIngredient = await _unitOfWork.IngredientRepository.CreateAsync(newIngredient);
			await _unitOfWork.SaveAsync();
			return newIngredient;
		}
	}
}
