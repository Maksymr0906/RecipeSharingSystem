import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const roles = localStorage.getItem('roles')?.split(',') || [];

  if (roles.includes('Admin')) {
    return true;
  }

  router.navigate(['/']);
  return false;
};