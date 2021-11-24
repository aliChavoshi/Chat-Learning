import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { User } from 'src/app/_models/account';
import { IMember } from 'src/app/_models/member';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.css'],
})
export class EditMemberComponent implements OnInit {
  user: User;
  member: IMember;

  form: FormGroup;

  constructor(
    private accountService: AccountService,
    private memberService: MemberService
  ) {}

  ngOnInit(): void {
    this.loadUser();
    this.loadMember();
  }

  loadMember() {
    this.memberService
      .getMemberByUsername(this.user.userName)
      .subscribe((member) => {
        this.member = member;
        this.form = new FormGroup({
          city: new FormControl(member.city),
          country: new FormControl(member.country),
          knowAs: new FormControl(member.knowAs),
          birthday: new FormControl(member.birthday),
        });
      });
  }
  loadUser() {
    this.accountService.currentUser$.subscribe((user) => {
      this.user = user;
    });
  }
}
