import { PaginatedResult } from './../_models/pagination';
import { IMember, IMemberUpdate, Photo } from './../_models/member';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = environment.baseUrl;
  private members: IMember[] = [];
  paginationResult: PaginatedResult<IMember[]> = new PaginatedResult<
    IMember[]
  >();

  constructor(private http: HttpClient) {}

  getMembers(pageNumber: number, pageSize: number) {
    // if (this.members.length > 0) return of(this.members);
    let params = new HttpParams();
    if (pageNumber !== null && pageSize !== null) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());
    }
    return this.http
      .get<PaginatedResult<IMember[]>>(`${this.baseUrl}/users/getAllUsers`, {
        params,
      })
      .pipe(
        map((response) => {
          this.members = response.items;
          this.paginationResult = response;
          return response;
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
    return this.http.put<Photo>(
      `${this.baseUrl}/users/setMainPhoto/${photoId}`,
      {}
    );
  }
  deletePhoto(photoId: number) {
    return this.http.delete<Photo>(
      `${this.baseUrl}/users/deletePhoto/${photoId}`
    );
  }
}
