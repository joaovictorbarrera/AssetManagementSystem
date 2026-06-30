import { Component } from '@angular/core';
import { AuthService } from '../../../services/api/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html'
})
export class Navbar {
  constructor(public authService: AuthService) {}
}
