import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  emailAddress = new FormControl("")

  constructor(public authService: AuthService, public router: Router) {}

  login(event: any) {
    event.preventDefault()

    let body = new FormData()
    body.set("emailAddress", this.emailAddress.value ?? "")

    this.authService.login(body)
    .subscribe({
      next: async () => {
        this.router.navigate(["/"])
      },
      error(err) {
        console.log(err)
      }
    })
  }
}
