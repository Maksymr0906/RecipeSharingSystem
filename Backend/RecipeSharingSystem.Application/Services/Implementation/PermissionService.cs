using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Interfaces.Repositories;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class PermissionService(IUnitOfWork unitOfWork)
	: IPermissionService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<HashSet<PermissionType>> GetPermissionsAsync(Guid userId)
	{
		return await _unitOfWork.UserRepository.GetPermissions(userId);
	}
}
