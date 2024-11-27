using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Ingredient;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Entities;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class IngredientService(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
	: IIngredientService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IValidationService _validationService = validationService;

	public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientRequestDto model)
	{
		await _validationService.ValidateAsync(model);

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
		await _validationService.ValidateAsync(model);

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

	// Consider changing to dto
	public async Task<Ingredient> GetOrCreateIngredientAsync(string ingredientName)
	{
		var existingIngredient = await _unitOfWork.IngredientRepository.GetByNameAsync(ingredientName);
		if (existingIngredient != null)
		{
			return existingIngredient;
		}

		var newIngredient = new Ingredient 
		{ 
			Name = ingredientName, 
			Slug = string.Join("-", ingredientName.ToLower().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)) 
		};
		
		newIngredient = await _unitOfWork.IngredientRepository.CreateAsync(newIngredient);
		await _unitOfWork.SaveAsync();
		return newIngredient;
	}

	public async Task<IngredientDto> GetIngredientBySlugAsync(string slug)
	{
		var ingredient = await _unitOfWork.IngredientRepository.GetBySlugAsync(slug);

		return _mapper.Map<IngredientDto>(ingredient);
	}
}
