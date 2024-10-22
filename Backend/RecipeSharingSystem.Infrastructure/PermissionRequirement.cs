using Microsoft.AspNetCore.Authorization;
using RecipeSharingSystem.Core.Enums;

namespace LearninPlatform.Infrastructure.Authentication;

public class PermissionRequirement(PermissionEnum[] permissions)
	: IAuthorizationRequirement
{
	public PermissionEnum[] Permissions { get; set; } = permissions;
}