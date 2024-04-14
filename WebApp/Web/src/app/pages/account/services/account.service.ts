import { AccountCreateForm, AccountCreateModel, AccountItem } from '@account/types/account.types';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  http = inject(HttpClient);

  getAccounts(): Observable<AccountItem[]> {
    return this.http.get<AccountItem[]>('/api/account/list');
  }

  buildCreateAccountForm(model: AccountCreateModel): AccountCreateForm {
    return new FormGroup({
      login: new FormControl(model.login, [Validators.required]),
      firstName: new FormControl(model.firstName, [Validators.required]),
      lastName: new FormControl(model.lastName, [Validators.required]),
      password: new FormControl(model.password, [Validators.required]),
      confirmPassword: new FormControl('', [Validators.required]),
    });
  }

  createAccount(model: AccountCreateModel): Observable<number> {
    return this.http.post<number>('/api/account/create', model);
  }
}

