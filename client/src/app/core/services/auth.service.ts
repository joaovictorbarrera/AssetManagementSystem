import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import User from '../DTOs/user.dto';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  readonly currentUser = signal<User | null>(null);

  private loadUserPromise?: Promise<User | null>;

  constructor(private http: HttpClient, private router: Router) {}

  login(body: any) {
    return this.http.post(`${this.apiUrl}/login`, body);
  }

  logout() {
    localStorage.removeItem('authorizationToken');
    this.currentUser.set(null);
    this.router.navigate(["/login"]);
  }

  async loadUser(): Promise<User | null> {
    if (this.currentUser()) {
      return this.currentUser();
    }

    const token = localStorage.getItem('authorizationToken');
    if (!token?.trim()) {
      return null;
    }

    if (this.loadUserPromise) {
      return this.loadUserPromise;
    }

    this.loadUserPromise = firstValueFrom(
      this.http.get<User>(`${this.apiUrl}/me`)
    )
      .then((user) => {
        this.currentUser.set(user);
        return user;
      })
      .catch(() => {
        this.logout();
        return null;
      })
      .finally(() => {
        this.loadUserPromise = undefined;
      });

    return this.loadUserPromise;
  }
}
