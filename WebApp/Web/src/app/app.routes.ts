import { Routes } from '@angular/router';
import { authGuard } from '@shared/guards/auth.guard';

export const routes: Routes = [
    { path: 'login', loadComponent: () => import('@login/components/login/login.component') },
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: '', loadComponent: () => import('@shared/components/layout/layout.component'), canActivateChild: [authGuard], children:[
        { path: 'dashboard', loadComponent: () => import('@dashboard/components/dashboard/dashboard.component') },
        { path: 'account', loadComponent: () => import('@account/components/account-list/account-list.component') },
    ] },
    { path: '**', loadComponent: () => import('@shared/components/not-found/not-found.component') },
];
