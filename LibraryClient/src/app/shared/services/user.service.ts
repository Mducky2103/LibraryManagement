import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

interface UserProfile {
  email: string;
  fullName: string;
  gender: string;
  dateOfBirth: string;
  roles: string[];
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient,
    private authService: AuthService
  ) { }

  getUserProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(environment.apiBaseUrl + '/UserProfile');
  }

  editUserProfile(data: any): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getToken()}`,
      'Content-Type': 'application/json'
    });
    return this.http.put(environment.apiBaseUrl + '/EditUserProfile', data, { headers });
  }
}
