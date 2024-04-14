import { AbstractControl, ValidationErrors } from '@angular/forms';

export function passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d](?=.*[!@#\$%\^&\*]).{8,}$/;

    if (control.value && !passwordRegex.test(control.value)) {
        return { weak: true };
    }

    return null;
}

export function confirmPasswordValidator(control: AbstractControl): ValidationErrors | null {
    const passwordControl = control.get('password');
    const confirmPasswordControl = control.get('confirmPassword');

    if (passwordControl && confirmPasswordControl && passwordControl.value !== confirmPasswordControl.value) {
        confirmPasswordControl.setErrors({ mismatch: true });
        return { mismatch: true };
    }

    return null;
}

