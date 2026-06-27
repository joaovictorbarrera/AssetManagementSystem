import { Component, Input } from '@angular/core'
import { DatePipe } from '@angular/common'
import { Dropdown } from '../../../../core/components/dropdown/dropdown'
import User from '../../../../core/DTOs/user.dto'
import { Role } from '../../../../core/enums/role'

@Component({
  selector: 'tr[app-user-row]',
  imports: [DatePipe, Dropdown],
  templateUrl: './user-row.html',
  styleUrl: './user-row.scss',
})
export class UserRow {
  @Input() user!: User
  @Input() roles: string[] = []

  toggleActive() {
    this.user.isActive = !this.user.isActive
  }

  handleRoleChange(role: string) {
    this.user.role = role as Role
  }
}
