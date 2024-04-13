import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '@shared/services/auth.service';
import { catchError, firstValueFrom, map, of, throwError } from 'rxjs';

export const authGuard: CanActivateFn = async (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  if (authService.hasToken()) {
    if (!authService.isExpired()) {
      return true;
    }
    return await firstValueFrom(authService.refreshToken().pipe(map((response: any) => {
      return true;
    }), catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        authService.removeToken();
        router.navigate(['login']);
        return of(false);
      }
      return throwError(() => error);
    })));
  }
  router.navigate(['login']);
  return false;
};
