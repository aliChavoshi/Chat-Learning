import { IMember } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { MemberService } from 'src/app/_services/member.service';
import { Observable } from 'rxjs';
import { IPagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-list-member',
  templateUrl: './list-member.component.html',
  styleUrls: ['./list-member.component.css'],
})
export class ListMemberComponent implements OnInit {
  members: IMember[] = [];
  pagination: IPagination;
  pageNumber = 1;
  pageSize = 6;

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.memberService
      .getMembers(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.members = response.items;
        this.pagination = response.pagination;
      });
  }
}
