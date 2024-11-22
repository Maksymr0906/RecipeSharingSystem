import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserPersonalInfo } from '../models/user-personal-info.model';
import { environment } from 'src/environments/environment.development';
import { EditUserRequest } from '../models/edit-user-request.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUserById(id: string): Observable<UserPersonalInfo> {
    return this.http.get<UserPersonalInfo>(`${environment.apiBaseUrl}/api/users/${id}`);
  }

  updateUser(id: string, model: EditUserRequest): Observable<void> {
    return this.http.put<void>(`${environment.apiBaseUrl}/api/users/${id}`, model);
  }
}
