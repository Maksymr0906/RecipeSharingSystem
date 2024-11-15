﻿using RecipeSharingSystem.Application.Services.Interfaces;
using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Interfaces.Repositories;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class PermissionService(IUserRepository usersRepository)
	: IPermissionService
{
	private readonly IUserRepository _usersRepository = usersRepository;

	public async Task<HashSet<PermissionType>> GetPermissionsAsync(Guid userId)
	{
		return await _usersRepository.GetPermissions(userId);
	}
}
