import { IUser } from './../_models/account';
import { AccountService } from 'src/app/_services/account.service';
import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { first } from 'rxjs';

@Directive({
  selector: '[appHasInRole]', //*appHasInRole('Admin','SuperAdmin')
})
export class HasInRoleDirective {
  user: IUser;

  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private accountService: AccountService
  ) {}

  @Input() set appHasInRole(roles: string[]) {
    this.accountService.currentUser$.pipe(first()).subscribe((user) => {
      this.user = user;
      if (!this.user) {
        this.viewContainerRef.clear();
        return;
      }
      if (!this.user?.roles && this.user) {
        this.viewContainerRef.clear();
        return;
      }
      if (this.user?.roles.some((role) => roles.includes(role))) {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.viewContainerRef.clear();
      }
    });
  }
  ngOnInit(): void {}
}
