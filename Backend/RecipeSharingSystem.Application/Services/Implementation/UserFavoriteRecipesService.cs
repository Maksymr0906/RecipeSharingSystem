using AutoMapper;
using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Business.DTOs.Recipe;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class UserFavoriteRecipesService(
	IUnitOfWork unitOfWork,
	IMapper mapper,
	IResourceAccessService resourceAccessService)
	: IUserFavoriteRecipesService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;
	private readonly IResourceAccessService _resourceAccessService = resourceAccessService;

	public async Task AddRecipeToFavorites(Guid userId, Guid recipeId)
	{
		await _resourceAccessService.EnsureOwnerOrPermissionAsync(userId, PermissionType.Update);

		await _unitOfWork.UserFavoriteRecipeRepository.AddRecipeToFavorites(userId, recipeId);

		await _unitOfWork.SaveAsync();
	}

	public async Task<ICollection<RecipeDto>> GetFavoriteRecipesForUser(Guid userId)
	{
		await _resourceAccessService.EnsureOwnerOrPermissionAsync(userId, PermissionType.Update);

		var recipes = await _unitOfWork.UserFavoriteRecipeRepository.GetFavoriteRecipesForUserAsync(userId);

		return _mapper.Map<ICollection<RecipeDto>>(recipes);
	}

	public async Task<bool> IsRecipeFavorite(Guid userId, Guid recipeId)
	{
		await _resourceAccessService.EnsureOwnerOrPermissionAsync(userId, PermissionType.Update);

		return await _unitOfWork.UserFavoriteRecipeRepository.IsFavorite(userId, recipeId);
	}

	public async Task RemoveRecipeFromFavorites(Guid userId, Guid recipeId)
	{
		await _resourceAccessService.EnsureOwnerOrPermissionAsync(userId, PermissionType.Update);

		await _unitOfWork.UserFavoriteRecipeRepository.RemoveRecipeFromFavorites(userId, recipeId);

		await _unitOfWork.SaveAsync();
	}
}
