import { IUser } from './../_models/account';
import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private toast: ToastrService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user: IUser) => {
        if (
          user.roles?.includes('admin') ||
          user.roles?.includes('superAdmin')
        ) {
          return true;
        }
        this.toast.error('You are not admin');
        return false;
      })
    );
  }
}
