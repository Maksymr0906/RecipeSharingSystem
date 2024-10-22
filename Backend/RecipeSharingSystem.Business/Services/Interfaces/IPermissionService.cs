using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Application.Services.Interfaces;

public interface IPermissionService
{
	Task<HashSet<PermissionEnum>> GetPermissionsAsync(Guid userId);
}
