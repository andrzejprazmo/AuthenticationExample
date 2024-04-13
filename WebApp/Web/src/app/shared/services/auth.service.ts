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
      console.log(this.jwtHelper.getTokenExpirationDate());
      return this.jwtHelper.isTokenExpired(token);
    }
    return true;
  }

  refreshToken(): Observable<any> {
    const http = new HttpClient(this.httpBackend);
    const currentToken = this.loadToken();
    return http.get(`api/auth/refresh-token/${currentToken}`).pipe(map((newToken: any) => {
      this.saveToken(newToken);
      return newToken;
    }));
  }
}
