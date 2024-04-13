import { HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@shared/services/auth.service';
import { Observable, catchError, filter, finalize, map, of, switchMap, take } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);

  if (authService.hasToken() && authService.isExpired()) {
    return handleRefreshToken(req, next, authService);
  }

  return next(req);
};

export function handleRefreshToken(req: HttpRequest<unknown>, next: HttpHandlerFn, authService: AuthService): Observable<any> {
  const router = inject(Router);
  if (!authService.refreshMode) {
    authService.refreshMode = true;
    authService.tokenSubject.next('');
    return authService.refreshToken().pipe(switchMap(newToken => {
      authService.tokenSubject.next(newToken);
      return next(req);
    }), catchError(err => {
      if (err.status === 401) {
        authService.removeToken();
        router.navigate(['login']);
      }
      return of(err);
    }), finalize(() => {
      authService.refreshMode = false;
    }))
  } else {
    return authService.tokenSubject.pipe(filter(token => token != ''), take(1), switchMap(token => {
      return next(req);
    }))
  }
}
