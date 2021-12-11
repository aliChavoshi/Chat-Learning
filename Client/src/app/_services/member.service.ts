import { IMember, IMemberUpdate } from './../_models/member';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.baseUrl;
  private members: IMember[] = [];

  constructor(private http: HttpClient) {}

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<IMember[]>(`${this.baseUrl}/users/getAllUsers`).pipe(
      tap((members) => {
        this.members = members;
      })
    );
  }
  getMemberByUsername(userName: string) {
    const member = this.members.find((x) => x.userName === userName);
    if (member !== undefined) return of(member);
    return this.http.get<IMember>(
      `${this.baseUrl}/users/getUserByUserName/${userName}`
    );
  }
  getMemberById(id: number) {
    const member = this.members.find((x) => x.id === id);
    if (member !== undefined) return of(member);
    return this.http.get<IMember>(`${this.baseUrl}/users/getUserById/${id}`);
  }
  updateMember(memberUpdate: IMemberUpdate) {
    return this.http
      .put<IMember>(`${this.baseUrl}/users/UpdateUser`, memberUpdate)
      .pipe(
        map((member) => {
          const index = this.members.findIndex((x) => x.id === member.id);
          this.members[index] = member;
          return member;
        })
      );
  }
  setMainPhoto(photoId: number) {
    return this.http.put(`${this.baseUrl}/users/setMainPhoto/${photoId}`, {});
  }
}
