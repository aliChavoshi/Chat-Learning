import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  Validator,
  Validators,
} from '@angular/forms';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MatchPasswordService implements Validator {
  constructor() {}

  validate(formGroup: FormGroup): ValidationErrors {
    const { password, passwordConfirm } = formGroup.value;
    if (password === passwordConfirm) {
      return null;
    }
    return { matchPassword: true };
  }
}
