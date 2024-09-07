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
    birthDate: Date | null;
}

export declare type AccountCreateForm = FormGroup<{
    login: FormControl<string | null>;
    firstName: FormControl<string | null>;
    lastName: FormControl<string | null>;
    password: FormControl<string | null>;
    confirmPassword: FormControl<string | null>;
    birthDate: FormControl<Date | null>;
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

export declare type AccountPasswordForm = FormGroup<{
    id: FormControl<number | null>;
    password: FormControl<string | null>;
    confirmPassword: FormControl<string | null>;
}>

export interface AccountPasswordModel {
    id: number;
    password: string;
}