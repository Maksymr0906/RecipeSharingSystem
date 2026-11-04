using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeSharingSystem.Core.Exceptions;

namespace RecipeSharingSystem.API.Extensions;

/// <summary>
/// Turns exceptions into predictable HTTP responses. Internal details (messages of unexpected
/// exceptions, stack traces, identifiers from repositories) are logged but never sent to the client.
///
/// It is a plain middleware placed after UseCors on purpose: the built-in UseExceptionHandler clears
/// the response headers, which would drop the CORS headers and make the browser hide the real status code.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
		{
			// The client went away; there is nobody to answer.
		}
		catch (Exception exception) when (!context.Response.HasStarted)
		{
			var (status, title, detail) = Map(exception);

			if (status == StatusCodes.Status500InternalServerError)
			{
				_logger.LogError(exception, "Unhandled exception while processing {Method} {Path}",
					context.Request.Method, context.Request.Path);
			}
			else if (exception is DbUpdateException)
			{
				_logger.LogWarning(exception, "Database constraint violation while processing {Method} {Path}",
					context.Request.Method, context.Request.Path);
			}

			context.Response.Clear();
			context.Response.StatusCode = status;

			await context.Response.WriteAsJsonAsync(new ProblemDetails
			{
				Status = status,
				Title = title,
				Detail = detail
			});
		}
	}

	private static (int Status, string Title, string? Detail) Map(Exception exception) => exception switch
	{
		ValidationException ex => (StatusCodes.Status400BadRequest, "Validation failed.", ex.Message),
		BadHttpRequestException ex => (ex.StatusCode, "Invalid request.", null),
		AuthenticationFailedException ex => (StatusCodes.Status401Unauthorized, "Authentication failed.", ex.Message),
		ForbiddenException ex => (StatusCodes.Status403Forbidden, "Access denied.", ex.Message),
		KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found.", null),
		DbUpdateException => (StatusCodes.Status409Conflict, "The request conflicts with existing data.", null),
		_ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", null)
	};
}
