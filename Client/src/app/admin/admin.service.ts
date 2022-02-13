import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IUser } from '../_models/account';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getUsersWithRoles() {
    return this.http.get<IUser[]>(`${this.baseUrl}/admin/getUsersWithRoles`);
  }
}
