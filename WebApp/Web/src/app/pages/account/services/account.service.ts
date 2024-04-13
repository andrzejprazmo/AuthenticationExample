import { AccountItem } from '@account/types/account.types';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  http = inject(HttpClient);

  getAccounts(): Observable<AccountItem[]> {
    return this.http.get<AccountItem[]>('/api/account/list');
  }
}
