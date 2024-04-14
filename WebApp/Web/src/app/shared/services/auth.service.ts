import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  tokenKey: string = 'Authorization';
  refreshMode: boolean = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

  httpBackend = inject(HttpBackend);
  jwtHelper = inject(JwtHelperService);

  saveToken(token: string) {
    sessionStorage.setItem(this.tokenKey, token);
  }

  loadToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  removeToken(): void {
    sessionStorage.removeItem(this.tokenKey);
  }

  hasToken(): boolean {
    return sessionStorage.getItem(this.tokenKey) != null;
  }

  isExpired(): boolean {
    const token = this.loadToken();
    if (token) {
      return this.jwtHelper.isTokenExpired(token);
    }
    return true;
  }

  refreshToken(): Observable<string> {
    const http = new HttpClient(this.httpBackend); // omitting interceptor
    return http.post<string>(`api/auth/refresh-token`, {
      token: this.loadToken()
    }).pipe(map((newToken: string) => {
      this.saveToken(newToken);
      return newToken;
    }));
  }
}
