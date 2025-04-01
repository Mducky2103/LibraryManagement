import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  createUser(formData: any) {
    //WARNING!
    //default value for Role
    //instead of registration form, there should some other
    //form to update these details of the user
    formData.role = 'Member';
    return this.http.post(environment.apiBaseUrl + '/signup', formData)
  }

  signin(formData: any) {
    return this.http.post(environment.apiBaseUrl + '/signin', formData)
  }

  isLoggedIn() {
    return this.getToken() != null ? true : false;
  }

  // forgotPassword1(data: any): Observable<any> {
  //   return this.http.post(environment.apiBaseUrl + '/forgot-password', data);
  // }

  forgotPassword(email: string) {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post(environment.apiBaseUrl + '/forgot-password', { email }, { headers });
  }

  resetPassword(payload: { email: string; token: string; newPassword: string }): Observable<any> {
    return this.http.post(environment.apiBaseUrl + '/reset-password', payload);
  }

  saveToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
  }

  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
  }

  getClaims() {
    return JSON.parse(window.atob(this.getToken()!.split('.')[1]))
  }
}
