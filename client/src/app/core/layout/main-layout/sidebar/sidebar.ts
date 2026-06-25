import { Component, computed } from '@angular/core';
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from '../../../services/auth.service';
import { Role } from '../../../enums/role';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, NgIcon],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss'
})
export class Sidebar {
  currentRole = computed(() => this.authService.currentUser()?.role)
  isManager = computed(() =>
    this.currentRole() === Role.AssetManager ||
    this.currentRole() === Role.Admin
  )
  isAdmin = computed(() => this.currentRole() === Role.Admin)

  constructor(public authService: AuthService) {}
}
