using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Application.Services.Interfaces;

public interface IPermissionService
{
	Task<HashSet<PermissionType>> GetPermissionsAsync(Guid userId);
}
