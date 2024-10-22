using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Repositories;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class PermissionService : IPermissionService
{
	private readonly IUserRepository _usersRepository;

	public PermissionService(IUserRepository usersRepository)
	{
		_usersRepository = usersRepository;
	}

	public async Task<HashSet<PermissionEnum>> GetPermissionsAsync(Guid userId)
	{
		return await _usersRepository.GetPermissions(userId);
	}
}
