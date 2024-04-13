import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginForm, LoginFormModel } from '@login/types/login.types';
import { AuthService } from '@shared/services/auth.service';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  http = inject(HttpClient);
  router = inject(Router);
  authService = inject(AuthService);

  buildLoginForm(model: LoginFormModel): LoginForm {
    return new FormGroup({
      login: new FormControl(model.login, [Validators.required]),
      password: new FormControl(model.password, [Validators.required])
    });
  }

  login(model: LoginFormModel): Observable<string> {
    return this.http.post<string>('/api/auth/login', model).pipe(map(result => {
      this.authService.saveToken(result);
      return result;
    }));
  }

  logout(): Observable<any> {
    return this.http.post<any>('api/auth/logout', null).pipe(map(() => {
      this.authService.removeToken();
    }));
  }

}
