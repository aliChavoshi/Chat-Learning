import { UserLikeParams } from './../../_enums/likeUser';
import { PaginatedResult } from './../../_models/pagination';
import { MemberService } from 'src/app/_services/member.service';
import { Component, OnInit } from '@angular/core';
import { IMember } from 'src/app/_models/member';
import { PredicateLikeEnum } from 'src/app/_enums/likeUser';

@Component({
  selector: 'app-home-list',
  templateUrl: './home-list.component.html',
  styleUrls: ['./home-list.component.css'],
})
export class HomeListComponent implements OnInit {
  result: PaginatedResult<IMember[]>;
  userLikeParams = new UserLikeParams();

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.memberService.getUserLikes(this.userLikeParams).subscribe((res) => {
      this.result = res;
      console.log(res);
    });
  }
  pageChanged(event: any) {
    this.result.currentPage = event?.page;
    this.userLikeParams.pageNumber = event?.page;
  }
}
