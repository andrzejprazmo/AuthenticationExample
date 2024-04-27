import { AccountService } from '@account/services/account.service';
import { AccountItem, AccountPasswordForm, AccountPasswordModel } from '@account/types/account.types';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { addFormErrors } from '@shared/helpers/error.helper';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-account-password',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './account-password.component.html',
  styleUrl: './account-password.component.css'
})
export class AccountPasswordComponent {
  accountService = inject(AccountService);
  account!: AccountItem;
  form!: AccountPasswordForm;
  modalRef: BsModalRef = inject(BsModalRef);

  ngOnInit() {
    this.form = this.accountService.buildSetPasswordForm(this.account.id);
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.accountService.setPassword(this.mapToModel(this.form)).subscribe({
      next: () => { 
        this.modalRef.onHide?.next('SUCCESS');
        this.modalRef.hide();
      },
      error: (err: HttpErrorResponse) => {
        addFormErrors(this.form, err);
      }
    })
  }

  mapToModel(form: AccountPasswordForm): AccountPasswordModel {
    return {
      id: form.controls.id.value || 0 as number,
      password: form.controls.password.value || '' as string,
    }
  }
}
