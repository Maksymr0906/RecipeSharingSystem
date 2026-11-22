using RecipeSharingSystem.Core.Enums;
using RecipeSharingSystem.Core.Exceptions;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Application.Services.Implementation;

public class ResourceAccessService(
	ICurrentUser currentUser,
	IPermissionService permissionService,
	ISecurityEventLogger securityEventLogger)
	: IResourceAccessService
{
	private readonly ICurrentUser _currentUser = currentUser;
	private readonly IPermissionService _permissionService = permissionService;
	private readonly ISecurityEventLogger _securityEventLogger = securityEventLogger;

	public async Task EnsureOwnerOrPermissionAsync(Guid ownerId, PermissionType overridePermission)
	{
		// The caller identity always comes from the validated token, never from the request body.
		var callerId = _currentUser.UserId;

		if (callerId == ownerId)
		{
			return;
		}

		var permissions = await _permissionService.GetPermissionsAsync(callerId);

		if (permissions.Contains(overridePermission))
		{
			return;
		}

		_securityEventLogger.AuthorizationDenied(callerId, $"owner:{ownerId}", overridePermission.ToString());

		throw new ForbiddenException("You do not have permission to access this resource.");
	}
}
