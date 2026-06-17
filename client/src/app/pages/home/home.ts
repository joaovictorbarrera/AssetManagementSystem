import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  loggedInEmailAddress: WritableSignal<string | null> = signal(null)

  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    this.authService.me()
    .subscribe({
      next: (res: any) => {
        if (res && res.emailAddress) {
          this.loggedInEmailAddress.set(res.emailAddress)
        }
      },
      error: () => {
        this.loggedInEmailAddress.set(null)
      }
    })
  }
}
