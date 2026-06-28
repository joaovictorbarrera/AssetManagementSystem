import { Component, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from '../../../services/auth.service';
import { Role } from '../../../enums/role';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, RouterLink, RouterLinkActive, NgIcon],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss'
})
export class Sidebar {
  currentRole = computed(() => this.authService.currentUser()?.role)

  constructor(public authService: AuthService) {}
}
