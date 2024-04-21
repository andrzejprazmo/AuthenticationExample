import { AccountService } from '@account/services/account.service';
import { AccountEditModel } from '@account/types/account.types';
import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';

export const accountEditResolver: ResolveFn<AccountEditModel | null> = (route, state) => {
  const accountService = inject(AccountService);
  const accountId = route.params['id'];
  if (accountId) {
    return accountService.editAccount(accountId);
  }
  return null;
};
