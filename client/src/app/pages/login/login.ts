import { Component, signal, WritableSignal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import LoginRequestDto from '../../DTOs/login-request.dto';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  emailAddress = new FormControl("")
  failMessage: WritableSignal<string | null> = signal(null)

  constructor(public authService: AuthService, public router: Router) {}

  login(event: any) {
    event.preventDefault()

    let body: LoginRequestDto = { emailAddress: this.emailAddress.value ?? "" }

    this.authService.login(body)
    .subscribe({
      next: (response: any) => {
        localStorage.setItem(
        "authorizationToken",
        response.authorizationToken);

        this.router.navigate(["/"])
      },
      error: err => {
        this.failMessage.set("Unable to login with this email.")
      }
    })
  }
}
