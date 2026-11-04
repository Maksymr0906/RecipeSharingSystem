using Microsoft.AspNetCore.Http;
using RecipeSharingSystem.Core.Exceptions;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;

namespace RecipeSharingSystem.Infrastructure.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public bool IsAuthenticated => TryGetUserId(out _);

	public Guid UserId =>
		TryGetUserId(out var userId)
			? userId
			: throw new AuthenticationFailedException("Authentication is required.");

	private bool TryGetUserId(out Guid userId)
	{
		userId = default;

		var claim = _httpContextAccessor.HttpContext?.User.FindFirst(CustomClaims.UserId);

		return claim is not null && Guid.TryParse(claim.Value, out userId);
	}
}
