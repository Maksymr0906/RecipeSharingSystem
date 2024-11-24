using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Core.Interfaces.Services;

public interface IPermissionService
{
	Task<HashSet<PermissionType>> GetPermissionsAsync(Guid userId);
}
