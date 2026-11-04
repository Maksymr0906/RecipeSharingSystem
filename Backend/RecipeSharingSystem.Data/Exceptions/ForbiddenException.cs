namespace RecipeSharingSystem.Core.Exceptions;

/// <summary>
/// Thrown when an authenticated caller is not allowed to perform an operation
/// on a resource (for example, editing a recipe that belongs to another user).
/// Mapped to HTTP 403.
/// </summary>
public class ForbiddenException(string message) : Exception(message);
