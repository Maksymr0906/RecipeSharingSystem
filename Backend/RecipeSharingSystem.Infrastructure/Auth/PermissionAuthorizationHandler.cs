using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Core.Interfaces.Services;

namespace RecipeSharingSystem.Infrastructure.Auth;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
            x => x.Type == CustomClaims.UserId);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
            return;

        using var scope = _scopeFactory.CreateScope();

        var permissionService = scope.ServiceProvider
            .GetRequiredService<IPermissionService>();

        var permissions = await permissionService.GetPermissionsAsync(id);

        if (permissions.Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
        }
        else
        {
            // Policy-based denial (as opposed to the ownership checks in ResourceAccessService):
            // the caller is authenticated but holds none of the permissions the endpoint requires.
            var securityEventLogger = scope.ServiceProvider.GetRequiredService<ISecurityEventLogger>();
            securityEventLogger.AuthorizationDenied(id, "policy", string.Join(",", requirement.Permissions));
        }
    }
}