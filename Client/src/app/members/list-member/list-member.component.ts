import { Gender, IMember, UserParams } from './../../_models/member';
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
  userParams = new UserParams();
  pageNumber = 1;
  pageSize = 6;

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.loadMembers();
  }
  pageChanged(event: any): void {
    this.pageNumber = event.page;
    this.loadMembers();
  }
  private loadMembers() {
    this.memberService.getMembers(this.userParams).subscribe((response) => {
      this.result = response;
    });
  }
}
