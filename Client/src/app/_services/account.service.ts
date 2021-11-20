import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IRequestLogin, IRequestRegister, User } from '../_models/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private baseUrl = environment.baseUrl;
  private currentUser = new BehaviorSubject<User>(null); // for next this service
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

  register(register: IRequestRegister) {
    return this.http
      .post<User>(`${this.baseUrl}/account/register`, register)
      .pipe(
        map((response) => {
          if (response.userName && response.token) {
            localStorage.setItem('user', JSON.stringify(response));
            this.currentUser.next(response);
          }
          return response;
        })
      );
  }

  setCurrentUser(user: User) {
    this.currentUser.next(user);
  }

  getCurrentUser() {
    return this.currentUser.getValue();
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.next(null);
  }
}
