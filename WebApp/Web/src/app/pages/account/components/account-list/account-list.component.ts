import { AccountService } from '@account/services/account.service';
import { AccountItem } from '@account/types/account.types';
import { Component, afterNextRender, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { AccountPasswordComponent } from '../account-password/account-password.component';
import { AccountRemoveComponent } from '../account-remove/account-remove.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-account-list',
  standalone: true,
  imports: [RouterModule, ModalModule, ToastrModule],
  providers: [BsModalService],
  templateUrl: './account-list.component.html',
  styleUrl: './account-list.component.css'
})
export default class AccountListComponent {

  subscriptions: Subscription[] = [];
  accountService = inject(AccountService);
  modal = inject(BsModalService);
  toastr = inject(ToastrService);

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

  setPassword(item: AccountItem) {
    const initialState = {
      account: item
    };
    this.modal.show(AccountPasswordComponent, { initialState }).onHide?.subscribe(result => {
      if (result === 'SUCCESS') {
        this.toastr.success('Password updated successfully');
      }
    });
  }

  removeAccount(item: AccountItem) {
    const initialState = {
      account: item
    };
    this.modal.show(AccountRemoveComponent, { initialState }).onHide?.subscribe(result => {
      if (result === 'SUCCESS') {
        this.toastr.success('Account has been removed successfully');
        this.getList();
      }
    });
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
