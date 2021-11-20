import { IMember } from './../_models/member';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  getMembers() {
    return this.http.get<IMember[]>(`${this.baseUrl}/users`);
  }
  getMemberByUsername(userName: string) {
    return this.http.get<IMember>(
      `${this.baseUrl}/users/getUserByUserName/${userName}`
    );
  }
  getMemberById(id: number) {
    return this.http.get<IMember>(`${this.baseUrl}/users/getUserById/${id}`);
  }
}
