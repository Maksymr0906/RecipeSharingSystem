using Microsoft.Extensions.Logging;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;

namespace RecipeSharingSystem.Infrastructure.Logging;

/// <summary>
/// See <see cref="ISecurityEventLogger"/>. Every method writes a single structured log entry with a
/// fixed "EventType" property and the message template kept constant per event, so entries of the same
/// kind are easy to filter, count and alert on (lab 7-8, step 10: "перелік подій, що підлягають реєстрації").
/// Nothing here ever receives or logs a password, token or key - only identifiers.
/// </summary>
public class SecurityEventLogger(ILogger<SecurityEventLogger> logger) : ISecurityEventLogger
{
	private readonly ILogger<SecurityEventLogger> _logger = logger;

	public void AuthenticationSucceeded(Guid userId)
	{
		using var _ = _logger.BeginScope(new Dictionary<string, object> { ["EventType"] = "AuthenticationSucceeded" });
		_logger.LogInformation("Authentication succeeded for user {UserId}", userId);
	}

	public void AuthenticationFailed(string emailAttempted, string reason)
	{
		using var _ = _logger.BeginScope(new Dictionary<string, object> { ["EventType"] = "AuthenticationFailed" });
		_logger.LogWarning("Authentication failed for {EmailAttempted}: {Reason}", emailAttempted, reason);
	}

	public void AuthorizationDenied(Guid callerId, string resource, string requiredPermission)
	{
		using var _ = _logger.BeginScope(new Dictionary<string, object> { ["EventType"] = "AuthorizationDenied" });
		_logger.LogWarning(
			"User {UserId} was denied access to {Resource} (required permission: {RequiredPermission})",
			callerId, resource, requiredPermission);
	}

	public void RateLimitRejected(string clientKey, string path)
	{
		using var _ = _logger.BeginScope(new Dictionary<string, object> { ["EventType"] = "RateLimitRejected" });
		_logger.LogWarning("Request from {ClientKey} to {Path} was rejected by the rate limiter", clientKey, path);
	}

	public void AdminOperation(Guid actingUserId, string operation, string? targetDescription = null)
	{
		using var _ = _logger.BeginScope(new Dictionary<string, object> { ["EventType"] = "AdminOperation" });
		_logger.LogInformation(
			"Admin operation {Operation} performed by {UserId}{TargetSuffix}",
			operation, actingUserId, targetDescription is null ? "" : $" on {targetDescription}");
	}
}
