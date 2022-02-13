import { HttpClient, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IRequestLogin, IRequestRegister, IUser } from '../_models/account';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private baseUrl = environment.baseUrl;
  private currentUser = new BehaviorSubject<IUser>(null); // for next this service
  currentUser$ = this.currentUser.asObservable(); // for subscribe

  constructor(private http: HttpClient) {}

  login(login: IRequestLogin) {
    return this.http.post<IUser>(`${this.baseUrl}/account/login`, login).pipe(
      map((response: IUser) => {
        if (response.userName && response.token) {
          this.setCurrentUser(response);
        }
      })
    );
  }

  register(register: IRequestRegister) {
    return this.http
      .post<IUser>(`${this.baseUrl}/account/register`, register)
      .pipe(
        map((response) => {
          if (response.userName && response.token) {
            this.setCurrentUser(response);
          }
          return response;
        })
      );
  }

  isExistUserName(userName: string) {
    return this.http.get<boolean>(
      `${this.baseUrl}/account/isExistUsername/${userName}`
    );
  }

  setCurrentUser(user: IUser) {
    if (user) {
      user.roles = [];
      const roles = this.getDecodedToken(user.token)?.role;
      if (roles)
        Array.isArray(roles) ? (user.roles = roles) : user.roles.push(roles);
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUser.next(user);
    }
  }

  getCurrentUser() {
    return this.currentUser.getValue();
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.next(null);
  }

  private getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
