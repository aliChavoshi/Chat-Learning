import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

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

  constructor(private http: HttpClient) {}

  login(login: IRequestLogin) {
    return this.http.post<User>(`${this.baseUrl}/account/login`, login);
  }
}
