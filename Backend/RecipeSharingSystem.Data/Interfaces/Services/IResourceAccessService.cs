using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Core.Interfaces.Services;

/// <summary>
/// Resource-level authorization: complements the role/permission policies on controllers
/// with the ownership rule from docs/architecture/business-logic.md (owner or administrator).
/// </summary>
public interface IResourceAccessService
{
	/// <summary>
	/// Succeeds if the current user is the owner of the resource, or holds
	/// <paramref name="overridePermission"/> (an administrator-level permission).
	/// </summary>
	/// <exception cref="RecipeSharingSystem.Core.Exceptions.ForbiddenException">Access is denied.</exception>
	Task EnsureOwnerOrPermissionAsync(Guid ownerId, PermissionType overridePermission);
}
