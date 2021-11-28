import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { IPreventUnsavedChanges } from 'src/app/_guards/prevent-unsaved-changes.guard';
import { User } from 'src/app/_models/account';
import { IMember } from 'src/app/_models/member';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.css'],
})
export class EditMemberComponent implements OnInit, IPreventUnsavedChanges {
  user: User;
  member: IMember;

  form: FormGroup;

  constructor(
    private accountService: AccountService,
    private memberService: MemberService
  ) {}

  canDeactivate(): boolean | Observable<boolean> {
    return this.form.dirty
      ? confirm('تغییرات را ذخیره نکرده اید میخواهید خارج شودید ؟')
      : true;
  }

  ngOnInit(): void {
    this.loadUser();
    this.loadMember();
  }
  OnSubmit() {
    console.log(this.form.value);
  }
  loadMember() {
    this.memberService
      .getMemberByUsername(this.user.userName)
      .subscribe((member) => {
        this.member = member;
        this.form = new FormGroup({
          city: new FormControl(member.city),
          country: new FormControl(member.country),
          knownAs: new FormControl(member.knownAs),
          dateOfBirth: new FormControl(member.dateOfBirth),
          email: new FormControl(member.email),
          interests: new FormControl(member.interests),
          lookingFor: new FormControl(member.lookingFor),
          introduction: new FormControl(member.introduction),
        });
      });
  }
  loadUser() {
    this.accountService.currentUser$.subscribe((user) => {
      this.user = user;
    });
  }
}
