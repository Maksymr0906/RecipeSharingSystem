import { HttpErrorResponse } from '@angular/common/http';

/**
 * Turns an error returned by the API into a short text for the user.
 * The API answers with either a plain string (controllers: BadRequest(message))
 * or with a ProblemDetails object (ExceptionHandlingMiddleware, rate limiter).
 */
export function extractErrorMessage(error: unknown): string {
  if (!(error instanceof HttpErrorResponse)) {
    return 'Unexpected error.';
  }

  if (error.status === 0) {
    return 'The server is not reachable. Check your connection and try again.';
  }

  const body = error.error;

  if (typeof body === 'string' && body.trim().length > 0 && body.length < 400) {
    return body;
  }

  if (body && typeof body === 'object') {
    if (typeof body.detail === 'string' && body.detail) {
      return body.detail;
    }

    // ASP.NET model-binding errors: { errors: { Field: ['message', ...] } }
    if (body.errors && typeof body.errors === 'object') {
      const messages = Object.values(body.errors as Record<string, string[]>).flat();
      if (messages.length > 0) {
        return messages.join(' ');
      }
    }

    if (typeof body.title === 'string' && body.title) {
      return body.title;
    }
  }

  switch (error.status) {
    case 400: return 'The request is not valid.';
    case 401: return 'You need to log in.';
    case 403: return 'You do not have permission to do this.';
    case 404: return 'The requested item was not found.';
    case 409: return 'The request conflicts with existing data.';
    case 429: return 'Too many requests. Please wait a minute and try again.';
    default: return 'Something went wrong. Please try again later.';
  }
}
