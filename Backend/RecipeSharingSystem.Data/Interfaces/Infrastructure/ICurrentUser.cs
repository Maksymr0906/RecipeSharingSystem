namespace RecipeSharingSystem.Core.Interfaces.Infrastructure;

/// <summary>
/// The identity of the caller of the current request, taken from the validated JWT on the server.
/// Services must use this instead of identifiers sent by the client.
/// </summary>
public interface ICurrentUser
{
	bool IsAuthenticated { get; }

	/// <exception cref="RecipeSharingSystem.Core.Exceptions.AuthenticationFailedException">
	/// The request is not authenticated.
	/// </exception>
	Guid UserId { get; }
}
