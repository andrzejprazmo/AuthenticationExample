import { FormControl, FormGroup } from "@angular/forms";

export interface LoginFormModel {
    login: string;
    password: string;
}
export declare type LoginForm = FormGroup<{
    login: FormControl<string | null>;
    password: FormControl<string | null>;
}>;
