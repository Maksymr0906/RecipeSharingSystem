namespace RecipeSharingSystem.Core.Exceptions;

/// <summary>
/// Thrown when the caller could not be authenticated (bad credentials, missing identity).
/// Mapped to HTTP 401. The message must stay generic and never reveal which part failed.
/// </summary>
public class AuthenticationFailedException(string message) : Exception(message);
