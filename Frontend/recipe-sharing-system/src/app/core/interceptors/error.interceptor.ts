import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from 'src/app/features/auth/services/auth.service';
import { NotificationService } from '../services/notification.service';
import { extractErrorMessage } from '../utils/http-error';

/**
 * One place for the error answers of the API:
 *  401 (outside /api/auth) - the token is missing, invalid or expired: log out and go to the login page;
 *  400, 403, 409, 429, 5xx, no connection - show a message;
 *  404 - left to the component (for example "this user has no review yet" is not an error).
 * Requests to /api/auth/* are skipped: the login and registration pages show their own messages.
 * The error is always re-thrown, so a component can still react to it.
 */
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private notifications: NotificationService,
    private router: Router
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse && !request.url.includes('/api/auth/')) {
          this.handle(error);
        }
        return throwError(() => error);
      })
    );
  }

  private handle(error: HttpErrorResponse): void {
    if (error.status === 404) {
      return;
    }

    if (error.status === 401) {
      this.authService.logout();
      this.notifications.warning('Your session has expired or you are not logged in. Please log in.');
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }

    this.notifications.error(extractErrorMessage(error));
  }
}
