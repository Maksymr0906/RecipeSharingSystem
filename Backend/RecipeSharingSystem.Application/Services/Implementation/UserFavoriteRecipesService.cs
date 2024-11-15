using AutoMapper;
using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class UserFavoriteRecipesService(IUnitOfWork unitOfWork, IMapper mapper)
	: IUserFavoriteRecipesService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;

	public async Task AddRecipeToFavorites(Guid userId, Guid recipeId)
	{
		await _unitOfWork.UserFavoriteRecipeRepository.AddRecipeToFavorites(userId, recipeId);
		await _unitOfWork.SaveAsync();
	}

	public async Task<ICollection<RecipeDto>> GetFavoriteRecipesForUser(Guid userId)
	{
		var recipes = await _unitOfWork.UserFavoriteRecipeRepository.GetFavoriteRecipesForUserAsync(userId);
		return _mapper.Map<ICollection<RecipeDto>>(recipes);
	}

	public async Task<bool> IsRecipeFavorite(Guid userId, Guid recipeId)
	{
		return await _unitOfWork.UserFavoriteRecipeRepository.IsFavorite(userId, recipeId);
	}

	public async Task RemoveRecipeFromFavorites(Guid userId, Guid recipeId)
	{
		await _unitOfWork.UserFavoriteRecipeRepository.RemoveRecipeFromFavorites(userId, recipeId);
		await _unitOfWork.SaveAsync();
	}
}
