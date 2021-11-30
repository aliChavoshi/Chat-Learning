import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanDeactivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';

export interface IPreventUnsavedChanges {
  canDeactivate(): Observable<boolean> | boolean;
}

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard
  implements CanDeactivate<IPreventUnsavedChanges>
{
  canDeactivate(
    component: IPreventUnsavedChanges,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot
  ): Observable<boolean> | boolean {
    return component.canDeactivate();
  }
}
