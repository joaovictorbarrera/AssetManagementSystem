import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`

  constructor(private http: HttpClient) {}

  login(body: any) {
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
