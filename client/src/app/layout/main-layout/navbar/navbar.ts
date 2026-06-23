import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import User from '../../../DTOs/user.dto';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html'
})
export class Navbar implements OnInit {
    loggedInUser: WritableSignal<User | null> = signal(null)

  constructor(public authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.me()
    .subscribe({
      next: (res: any) => {
        this.loggedInUser.set(res as User)
      },
      error: () => {
        this.loggedInUser.set(null)
        this.router.navigate(["/login"])
      }
    })
  }

  logout() {
    localStorage.removeItem("authorizationToken");

    this.loggedInUser.set(null)
    this.router.navigate(["/login"])
  }
}
