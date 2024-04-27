import { AccountService } from '@account/services/account.service';
import { AccountItem } from '@account/types/account.types';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-account-remove',
  standalone: true,
  imports: [],
  templateUrl: './account-remove.component.html',
  styleUrl: './account-remove.component.css'
})
export class AccountRemoveComponent {
  accountService = inject(AccountService);
  account!: AccountItem;
  modalRef: BsModalRef = inject(BsModalRef);

  removeAccount(id: number) {
    this.accountService.removeAccount(id).subscribe({
      next: () => {
        this.modalRef.hide();
        this.modalRef.onHide?.next('SUCCESS');
      },
      error: (err: HttpErrorResponse) => {
        // TODO handling errors
      }
    })
  }
}
