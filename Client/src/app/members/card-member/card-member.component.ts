import { ToastrService } from 'ngx-toastr';
import { MemberService } from 'src/app/_services/member.service';
import { IMember } from './../../_models/member';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card-member',
  templateUrl: './card-member.component.html',
  styleUrls: ['./card-member.component.css'],
})
export class CardMemberComponent implements OnInit {
  @Input() member: IMember;

  constructor(
    private memberService: MemberService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {}
  AddLike() {
    this.memberService.addLike(this.member.userName).subscribe(() => {
      this.toast.success('user Like' + ' ' + this.member.userName);
    });
  }
}
