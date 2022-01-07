import {
  Gender,
  IMember,
  UserParams,
  OrderBy,
  TypeSort,
} from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { MemberService } from 'src/app/_services/member.service';
import { PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-list-member',
  templateUrl: './list-member.component.html',
  styleUrls: ['./list-member.component.css'],
})
export class ListMemberComponent implements OnInit {
  result: PaginatedResult<IMember[]>;
  userParams: UserParams;
  //for view
  genders = Gender;
  orderBy = OrderBy;
  typeSort = TypeSort;

  constructor(private memberService: MemberService) {
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }
  pageChanged(event: any): void {
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    this.loadMembers();
  }
  private loadMembers() {
    this.memberService.getMembers(this.userParams).subscribe((response) => {
      this.result = response;
    });
  }
  onClear() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }
  onSubmit() {
    this.loadMembers();
  }
}
