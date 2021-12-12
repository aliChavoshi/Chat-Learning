import { Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  ValidationErrors,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root',
})
export class UniqueUserNameService implements AsyncValidator {
  constructor(private accountService: AccountService) {}

  validate(
    control: AbstractControl
  ): Promise<ValidationErrors> | Observable<ValidationErrors> {
    throw new Error('Method not implemented.');
  }
}
