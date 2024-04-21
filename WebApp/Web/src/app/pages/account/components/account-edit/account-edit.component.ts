import { AccountService } from '@account/services/account.service';
import { AccountEditForm, AccountEditModel } from '@account/types/account.types';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { addFormErrors } from '@shared/helpers/error.helper';

@Component({
  selector: 'app-account-edit',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './account-edit.component.html',
  styleUrl: './account-edit.component.css'
})
export default class AccountEditComponent {

  form!: AccountEditForm;
  activatedRoute = inject(ActivatedRoute);
  accountService = inject(AccountService);
  router = inject(Router);

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.form = this.accountService.buildEditAccountForm(data['account'])
    })
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.accountService.updateAccount(this.mapToModel(this.form)).subscribe({
      next: () => { 
        this.router.navigate(['account']);
      },
      error: (err: HttpErrorResponse) => {
        addFormErrors(this.form, err);
      }

    })
  }

  mapToModel(form: AccountEditForm): AccountEditModel {
    return {
      login: form.controls.login.value || '' as string,
      firstName: form.controls.firstName.value || '' as string,
      lastName: form.controls.lastName.value || '' as string,
      id: form.controls.id.value || 0 as number,
    }
  }
}
