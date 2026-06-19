import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import LoginRequestDto from '../DTOs/login-request.dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`

  constructor(private http: HttpClient) {}

  login(body: LoginRequestDto) {
    return this.http
      .post(`${this.apiUrl}/login`, body, {
        withCredentials: true
      })
  }

  me() {
    return this.http.get(`${this.apiUrl}/me`, {
      withCredentials: true
    })
  }
}
