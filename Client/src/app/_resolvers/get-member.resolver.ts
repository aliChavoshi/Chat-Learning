import { Injectable } from '@angular/core';
import {
  Router,
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { IMember } from '../_models/member';
import { MemberService } from '../_services/member.service';

@Injectable({
  providedIn: 'root',
})
export class GetMemberResolver implements Resolve<IMember> {
  constructor(private memberService: MemberService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<IMember> {
    return this.memberService.getMemberByUsername(route.params['username']);
  }
}
