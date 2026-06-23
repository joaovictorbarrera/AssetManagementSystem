import { Component, signal, WritableSignal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { SubmitButton } from "../../components/submit-button/submit-button";
@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule, SubmitButton],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  emailAddress = new FormControl("")
  failMessage: WritableSignal<string | null> = signal(null)
  loading = signal(false)

  constructor(public authService: AuthService, public router: Router) {}

  login(event: any) {
    event.preventDefault()

    let body = { emailAddress: this.emailAddress.value ?? "" }

    this.loading.set(true)
    this.authService.login(body)
    .subscribe({
      next: (response: any) => {
        this.loading.set(false)

        localStorage.setItem(
        "authorizationToken",
        response.authorizationToken);

        this.router.navigate(["/"])
      },
      error: err => {
        this.loading.set(false)
        this.failMessage.set("Unable to login with this email.")
      }
    })
  }
}
