import { AccountService } from './../_services/account.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MatchPasswordService } from '../_validators/match-password.service';
import { UniqueUserNameService } from '../_validators/unique-user-name.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() close = new EventEmitter();
  constructor(
    private accountService: AccountService,
    private router: Router,
    private toast: ToastrService,
    private matchPassword: MatchPasswordService,
    private uniqUserName: UniqueUserNameService
  ) {}

  form = new FormGroup(
    {
      userName: new FormControl(
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(10),
        ],
        [this.uniqUserName.validate.bind(this.uniqUserName)]
      ),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(20),
      ]),
      passwordConfirm: new FormControl('', [
        Validators.required,
        Validators.minLength(5),
        Validators.maxLength(20),
      ]),
      dateOfBirth: new FormControl('', [Validators.required]),
      gender: new FormControl(0, [Validators.required]), //radio select option
      knownAs: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(10),
      ]),
      city: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(10),
      ]),
      country: new FormControl('', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(10),
      ]),
    },
    { validators: [this.matchPassword.validate.bind(this.matchPassword)] }
  );

  ngOnInit(): void {}

  onSubmit() {
    this.accountService.register(this.form.value).subscribe((user) => {
      this.router.navigateByUrl('/members');
      this.toast.success('ورود شما با موفقیت انجام شد', 'موفقیت');
    });
  }
  cancel() {
    this.close.emit();
  }

  showValidatorForMatchPassword() {
    return (
      this.form.dirty &&
      this.form.get('password').touched &&
      this.form.get('passwordConfirm').touched &&
      this.form.get('password').dirty &&
      this.form.get('passwordConfirm').dirty &&
      this.form.errors
    );
  }
}
