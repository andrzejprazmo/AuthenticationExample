import { AccountService } from '@account/services/account.service';
import { AccountCreateForm, AccountCreateModel } from '@account/types/account.types';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import DatePickerComponent from '@shared/controls/date-picker/date-picker.component';
import { addFormErrors } from '@shared/helpers/error.helper';

@Component({
  selector: 'app-account-create',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, DatePickerComponent],
  templateUrl: './account-create.component.html',
  styleUrl: './account-create.component.css'
})
export default class AccountCreateComponent {

  accountService = inject(AccountService);
  router = inject(Router);

  form = this.accountService.buildCreateAccountForm({
    login: '',
    firstName: '',
    lastName: '',
    password: '',
    birthDate: null// new Date(2023, 0, 1)
  });

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.accountService.createAccount(this.mapToModel(this.form)).subscribe({
      next: (id) => {
        this.router.navigate(['account']);
      },
      error: (err: HttpErrorResponse) => {
        addFormErrors(this.form, err);
      }
    });
  }

  mapToModel(form: AccountCreateForm): AccountCreateModel {
    return {
      login: form.controls.login.value || '' as string,
      firstName: form.controls.firstName.value || '' as string,
      lastName: form.controls.lastName.value || '' as string,
      password: form.controls.password.value || '' as string,
      birthDate: form.controls.birthDate.value || null as Date | null
    }
  }
}
