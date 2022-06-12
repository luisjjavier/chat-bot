import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { UserRegistration } from '../models/user-registration';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl  = environment.apiUrl;
  constructor(private readonly http: HttpClient){ }

  registerUser(request: UserRegistration) {
    return this.http.post(`${this.baseUrl}/accounts/register`, request)
  }
  doLogin(request: LoginRequest){
    return this.http.post(`${this.baseUrl}/accounts/login`, request)
  }
}
