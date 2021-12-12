import {
  AbstractControl,
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

  validate(control: AbstractControl): ValidationErrors {
    const { password, passwordConfirm } = control.value;
    if (password === passwordConfirm) {
      return null;
    }
    return { matchPassword: true };
  }
}
