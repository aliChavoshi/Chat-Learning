import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  ValidationErrors,
} from '@angular/forms';
import {
  debounceTime,
  distinctUntilChanged,
  Observable,
  switchMap,
} from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root',
})
export class UniqueUserNameService implements AsyncValidator {
  constructor(private accountService: AccountService) {}

  validate(
    control: AbstractControl
  ): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return control.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap((value) => {
        return this.accountService.isExistUserName(value);
      }),
      map((response) => {
        if (!!response) control.setErrors({ uniqueUsername: true });
        return null;
      }),
      catchError((error) => {
        return null;
      })
    );
  }
}
