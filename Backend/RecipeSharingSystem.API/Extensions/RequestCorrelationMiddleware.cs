using Serilog.Context;

namespace RecipeSharingSystem.API.Extensions;

/// <summary>
/// Lab 7-8: gives every request a correlation id so log entries written by different components
/// (controllers, services, the exception middleware, Serilog's own request-logging middleware) while
/// handling the same request can be tied back together, as the theory section recommends for
/// distributed/multi-component systems. A client-supplied X-Correlation-Id is honoured so a request
/// can also be traced across the frontend and the API; otherwise a new id is generated.
/// </summary>
public class RequestCorrelationMiddleware(RequestDelegate next)
{
	private const string HeaderName = "X-Correlation-Id";

	private readonly RequestDelegate _next = next;

	public async Task InvokeAsync(HttpContext context)
	{
		var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var existing) && !string.IsNullOrWhiteSpace(existing)
			? existing.ToString()
			: Guid.NewGuid().ToString("n");

		context.Response.Headers[HeaderName] = correlationId;

		// Pushed into Serilog's ambient context: every log entry written anywhere while handling this
		// request (including by the exception middleware and security-event logger) picks it up automatically.
		using (LogContext.PushProperty("CorrelationId", correlationId))
		{
			await _next(context);
		}
	}
}
