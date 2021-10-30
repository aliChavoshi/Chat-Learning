import { AccountService } from './../_services/account.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

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
    private toast: ToastrService
  ) {}

  form = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
    ]),
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
  });

  ngOnInit(): void {}

  onSubmit() {
    this.accountService.register(this.form.value).subscribe(
      (user) => {
        this.router.navigateByUrl('/members');
        this.toast.success('ورود شما با موفقیت انجام شد', 'موفقیت');
      },
      (error) => {
        console.log(error);
      }
    );
  }
  cancel() {
    this.close.emit();
  }
}
