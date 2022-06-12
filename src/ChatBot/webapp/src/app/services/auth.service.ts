import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { CurrentUser } from '../models/current-user';
import { LoginRequest } from '../models/login-request';
import { UserRegistration } from '../models/user-registration';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl  = environment.apiUrl;
  constructor(private readonly http: HttpClient, private jwtHelper: JwtHelperService,

              private readonly router: Router){ }

  registerUser(request: UserRegistration) {
    return this.http.post(`${this.baseUrl}/accounts/register`, request)
  }
  doLogin(request: LoginRequest){
    return this.http.post(`${this.baseUrl}/accounts/login`, request)
  }

  getCurrentUser() {
    return this.jwtHelper.decodeToken(localStorage.getItem('token')!) as CurrentUser;
  }

  isAuthenticated() {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token')!);
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['']);
  }
}
