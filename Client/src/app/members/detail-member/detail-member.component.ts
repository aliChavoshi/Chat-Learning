import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IMember } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-detail-member',
  templateUrl: './detail-member.component.html',
  styleUrls: ['./detail-member.component.css'],
})
export class DetailMemberComponent implements OnInit {
  member: IMember;
  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.member = this.route.snapshot.data['member'] as IMember;
  }
}
