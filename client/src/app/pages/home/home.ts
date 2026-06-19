import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import User from '../../DTOs/user.dto';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  loggedInUser: WritableSignal<User | null> = signal(null)

  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    this.authService.me()
    .subscribe({
      next: (res: any) => {
        this.loggedInUser.set(res as User)
      },
      error: () => {
        this.loggedInUser.set(null)
      }
    })
  }

  logout() {
    localStorage.removeItem("authorizationToken");

    this.loggedInUser.set(null)
  }
}
