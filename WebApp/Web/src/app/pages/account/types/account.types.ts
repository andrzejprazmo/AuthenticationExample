import { FormControl, FormGroup } from "@angular/forms";

export interface AccountItem {
    id: number;
    login: string;
    firstName: string;
    lastName: string;
}

export interface AccountCreateModel {
    login: string;
    firstName: string;
    lastName: string;
    password: string;
}

export declare type AccountCreateForm = FormGroup<{
    login: FormControl<string | null>;
    firstName: FormControl<string | null>;
    lastName: FormControl<string | null>;
    password: FormControl<string | null>;
    confirmPassword: FormControl<string | null>;
}>

export interface AccountEditModel {
    id: number;
    login: string;
    firstName: string;
    lastName: string;
}

export declare type AccountEditForm = FormGroup<{
    id: FormControl<number | null>;
    login: FormControl<string | null>;
    firstName: FormControl<string | null>;
    lastName: FormControl<string | null>;
}>
