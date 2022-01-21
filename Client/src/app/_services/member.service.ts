import { PaginatedResult } from './../_models/pagination';
import { IMember, IMemberUpdate, Photo, UserParams } from './../_models/member';
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
  private userParams: UserParams = new UserParams();
  private cacheMember = new Map<string, PaginatedResult<IMember[]>>();
  paginationResult: PaginatedResult<IMember[]> = new PaginatedResult<
    IMember[]
  >();

  constructor(private http: HttpClient) {}

  getMembers(userParams: UserParams): Observable<PaginatedResult<IMember[]>> {
    const key = Object.values(userParams).join('-');
    //get from cache
    var response = this.cacheMember.get(key);
    if (response && response != null) return of(response);
    //get data from api
    let params = this.setParams(userParams);
    return this.http
      .get<PaginatedResult<IMember[]>>(`${this.baseUrl}/users/getAllUsers`, {
        params,
      })
      .pipe(
        map((response) => {
          this.members = response.items;
          this.paginationResult = response;
          //set to cache
          this.cacheMember.set(key, response);
          return response;
        })
      );
  }
  getMemberByUsername(userName: string) {
    let user = [...this.cacheMember]
      .reduce((arr, [key, value]) => arr.concat(value.items), [])
      .find((x) => x.userName === userName);
    if (user) return of(user);

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
  //user Likes
  addLike(targetUserName: string) {
    const params = new HttpParams().append('targetUserName', targetUserName);
    return this.http.post(`${this.baseUrl}/UserLikes/Add-Like`, {}, { params });
  }
  //user params
  setUserParams(userParams: UserParams) {
    this.userParams = userParams;
  }
  getUserParams() {
    return this.userParams;
  }
  resetUserParams() {
    this.userParams = new UserParams();
    return this.userParams;
  }
  private setParams(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.pageNumber !== null && userParams.pageSize !== null) {
      params = params.append('pageNumber', userParams.pageNumber.toString());
      params = params.append('pageSize', userParams.pageSize.toString());
      params = params.append('minAge', userParams.minAge.toString());
      params = params.append('maxAge', userParams.maxAge.toString());
      params = params.append('gender', userParams.gender.toString());
      params = params.append('orderBy', userParams.orderBy.toString());
      params = params.append('typeSort', userParams.typeSort.toString());
    }
    return params;
  }
}
