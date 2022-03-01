import { IMember } from './../../_models/member';
import { Component, Input, OnInit } from '@angular/core';
import { MemberService } from 'src/app/_services/member.service';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card-shared',
  templateUrl: './member-card-shared.component.html',
  styleUrls: ['./member-card-shared.component.css'],
})
export class MemberCardSharedComponent implements OnInit {
  @Input() member: IMember;

  constructor(
    private memberService: MemberService,
    private toast: ToastrService,
    public presence: PresenceService
  ) {}

  ngOnInit(): void {}
  AddLike() {
    this.memberService.addLike(this.member.userName).subscribe(() => {
      this.toast.success('user Like' + ' ' + this.member.userName);
    });
  }
}
