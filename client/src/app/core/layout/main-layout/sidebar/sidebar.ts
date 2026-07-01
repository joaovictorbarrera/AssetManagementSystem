import { Component, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from '../../../services/api/auth.service';
import { Role } from '../../../enums/role';
import { NgIcon } from '@ng-icons/core';
import { Labels } from '../../../constants/labels';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, RouterLink, RouterLinkActive, NgIcon],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss'
})
export class Sidebar implements OnInit {
  roleLabel = ''

  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    this.roleLabel = Labels.roles[this.authService.currentUser()!.role]
  }
}
