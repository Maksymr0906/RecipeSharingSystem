import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from '../services/auth.service';

/** Lets any logged-in user (with a not yet expired token) through; everybody else is sent to the login page. */
export const userGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieService);
  const authService = inject(AuthService);
  const router = inject(Router);

  const user = authService.getUser();
  const token = cookieService.get('Authorization').replace('Bearer ', '');

  if (user && token) {
    try {
      const { exp } = jwtDecode<{ exp: number }>(token);

      if (exp * 1000 > Date.now()) {
        return true;
      }
    } catch {
      // a malformed token is treated like a missing one
    }
  }

  authService.logout();
  return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
};
