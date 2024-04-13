import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '@login/services/login.service';
import { LoginForm, LoginFormModel } from '@login/types/login.types';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export default class LoginComponent {
  router = inject(Router);
  loginService = inject(LoginService);

  loginForm = this.loginService.buildLoginForm({
    login: '',
    password: ''
  });

  ngOnInit() {
    this.loginService.logout().subscribe();
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    this.loginService.login(this.mapToModel(this.loginForm)).subscribe({
      next: () => {
        this.router.navigate(['dashboard']);
      },
      error: (error: HttpErrorResponse) => {
        this.loginForm.setErrors({
          invalidCredentials: true
        });
      }
    });
  }

  mapToModel(form: LoginForm): LoginFormModel {
    return {
      login: form.controls.login.value || '' as string,
      password: form.controls.password.value || '' as string
    }
  }
}
