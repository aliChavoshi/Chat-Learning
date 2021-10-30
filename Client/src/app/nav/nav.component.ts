import { User } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  currentUser$: Observable<User>;

  form = new FormGroup({
    userName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(20),
    ]),
  });

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toast: ToastrService
  ) {}

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.accountService.login(this.form.value).subscribe(
      (user) => {
        this.router.navigateByUrl('/members');
        this.toast.success('ورود شما با موفقیت انجام شد', 'موفقیت');
      },
      (error: any) => {
        console.log(error);
      }
    );
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    this.toast.error('با موفقیت از سایت خارج شدید');
  }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }
}
