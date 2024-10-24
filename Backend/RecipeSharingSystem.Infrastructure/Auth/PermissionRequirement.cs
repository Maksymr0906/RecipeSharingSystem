using Microsoft.AspNetCore.Authorization;
using RecipeSharingSystem.Core.Enums;

namespace RecipeSharingSystem.Infrastructure.Auth;

public class PermissionRequirement(PermissionType[] permissions)
    : IAuthorizationRequirement
{
    public PermissionType[] Permissions { get; set; } = permissions;
}