using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class RecipeService : IRecipeService
	{
		private readonly IRecipeRepository _repository;
		private readonly IMapper _mapper;

		public RecipeService(IRecipeRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeRequestDto model)
		{
			var recipe = _mapper.Map<Recipe>(model);
			recipe = await _repository.CreateAsync(recipe);
			return _mapper.Map<RecipeDto>(recipe);
		}

		public async Task<ICollection<RecipeDto>> GetAllRecipesAsync()
		{
			var recipes = await _repository.GetAllAsync();
			return _mapper.Map<ICollection<RecipeDto>>(recipes);
		}

		public async Task<RecipeDto> GetRecipeByIdAsync(Guid id)
		{
			var recipe = await _repository.GetByIdAsync(id);
			return _mapper.Map<RecipeDto>(recipe);
		}

		public async Task<RecipeDto> UpdateRecipeAsync(Guid id, UpdateRecipeRequestDto model)
		{
			var recipe = _mapper.Map<Recipe>(model);
			recipe.Id = id;
			recipe = await _repository.UpdateAsync(recipe);
			return _mapper.Map<RecipeDto>(recipe);
		}

		public async Task<RecipeDto> DeleteRecipeAsync(Guid id)
		{
			var recipe = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<RecipeDto>(recipe);
		}
	}
}
