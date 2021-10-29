import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';

export interface IRequestLogin {
  userName: string;
  password: string;
}
export interface User {
  userName: string;
  token: string;
}
@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private baseUrl = 'https://localhost:5001/api';
  private currentUser = new ReplaySubject<User>(1); // for next this service
  currentUser$ = this.currentUser.asObservable(); // for subscribe

  constructor(private http: HttpClient) {}

  login(login: IRequestLogin) {
    return this.http.post<User>(`${this.baseUrl}/account/login`, login).pipe(
      map((response: User) => {
        if (response.userName && response.token) {
          localStorage.setItem('user', JSON.stringify(response));
          this.currentUser.next(response);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    this.currentUser.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.next(null);
  }
}
