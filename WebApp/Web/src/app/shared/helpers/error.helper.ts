import { HttpErrorResponse } from "@angular/common/http";
import { FormGroup } from "@angular/forms";

export function addFormErrors(form: FormGroup, error: HttpErrorResponse) {
    if (error.status === 400) {
        for (const key in error.error) {
            if (error.error.hasOwnProperty(key)) {
                const errorDetails = error.error[key];
                const propertyName = errorDetails.propertyName.charAt(0).toLowerCase() + errorDetails.propertyName.slice(1);
                const affectedControl = form.get(propertyName);
                if (affectedControl) {
                    affectedControl.setErrors({ [errorDetails.errorCode]: true });
                }
            }
        }
        return;
    }
    form.setErrors({ unknown: true });
}