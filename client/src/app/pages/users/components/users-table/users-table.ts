import { Component, Input } from '@angular/core'
import { TableHeader } from '../../../../core/components/table-components/table-header/table-header'
import PaginatedResponse from '../../../../core/DTOs/paginated.response'
import User from '../../../../core/DTOs/user.dto'
import { SpinningWheel } from '../../../../core/components/spinning-wheel/spinning-wheel'
import { UserRow } from '../user-row/user-row'
import { TableWrapper } from '../../../../core/components/table-components/table-wrapper/table-wrapper'

@Component({
  selector: 'app-users-table',
  imports: [TableHeader, SpinningWheel, UserRow, TableWrapper],
  templateUrl: './users-table.html',
  styleUrl: './users-table.scss',
})
export class UsersTable {
  @Input() users!: PaginatedResponse<User>
  @Input() headers: string[] = []
  @Input() roles: string[] = []
  @Input() loading: boolean = false
}
