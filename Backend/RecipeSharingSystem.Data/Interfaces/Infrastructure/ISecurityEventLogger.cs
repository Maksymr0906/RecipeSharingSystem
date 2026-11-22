namespace RecipeSharingSystem.Core.Interfaces.Infrastructure;

/// <summary>
/// Emits one structured, named record per security-relevant event (lab 7-8: logging and monitoring).
///
/// Every event is written through the normal logging pipeline (Serilog, see Program.cs) with a fixed
/// "EventType" property, so it can be filtered, counted and alerted on independently of free-text
/// messages elsewhere in the application. Implementations must never include a password, JWT, refresh
/// token, encryption key or other long-lived secret in the logged data - only identifiers (user id,
/// email used in the attempt, permission name, client key) needed to reconstruct "who did what, when,
/// with what result".
/// </summary>
public interface ISecurityEventLogger
{
	/// <summary>A caller successfully authenticated (issued a new access token).</summary>
	void AuthenticationSucceeded(Guid userId);

	/// <summary>
	/// A login attempt failed. <paramref name="emailAttempted"/> is logged because it is the audit
	/// subject of the event (which account is being targeted), not a secret - the password itself is
	/// never passed to this method.
	/// </summary>
	void AuthenticationFailed(string emailAttempted, string reason);

	/// <summary>An authenticated caller was denied access to a resource or operation they do not own or have permission for.</summary>
	void AuthorizationDenied(Guid callerId, string resource, string requiredPermission);

	/// <summary>A request was rejected by the rate limiter (candidate brute-force / abuse indicator).</summary>
	void RateLimitRejected(string clientKey, string path);

	/// <summary>An administrative or otherwise sensitive operation was performed (role change, PII export, key rotation, etc.).</summary>
	void AdminOperation(Guid actingUserId, string operation, string? targetDescription = null);
}
