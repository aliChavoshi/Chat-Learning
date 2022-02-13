import { AccountService } from './_services/account.service';
import { Component, OnInit } from '@angular/core';
import { IUser } from './_models/account';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Client';

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: IUser = JSON.parse(localStorage.getItem('user'));
    user
      ? this.accountService.setCurrentUser(user)
      : this.accountService.setCurrentUser(null);
  }
}
