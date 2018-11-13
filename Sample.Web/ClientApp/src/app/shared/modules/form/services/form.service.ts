import { NgForm } from '@angular/forms/';
import { Injectable } from '@angular/core';

import { Helpers } from '@shared/utilities/helpers';


@Injectable()
export class NkFormService {
    constructor(private Helpers: Helpers) {}

    public markFormAsTouched(form: NgForm) {
        if (!this.Helpers.isEmpty(form.controls)) {
            for (let controlName in form.controls) {
                form.controls[controlName].markAsTouched();
            }
        }
    }
    public isFormValid(form: NgForm) {
        return form.valid;
    }
}