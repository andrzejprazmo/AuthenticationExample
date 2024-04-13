import { AccountService } from '@account/services/account.service';
import { AccountItem } from '@account/types/account.types';
import { Component, inject } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-account-list',
  standalone: true,
  imports: [],
  templateUrl: './account-list.component.html',
  styleUrl: './account-list.component.css'
})
export default class AccountListComponent {

  subscriptions: Subscription[] = [];
  accountService = inject(AccountService);

  list: AccountItem[] = [];

  ngOnInit() {
    this.subscriptions.push(this.getList());
  }

  getList(): Subscription {
    return this.accountService.getAccounts().subscribe({
      next: items => {
        this.list = items;
      }
    })
  }

  ngOnDestroy(){
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
