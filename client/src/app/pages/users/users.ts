import { Component, OnInit, signal } from '@angular/core';
import { PageHeader } from '../components/page-header/page-header';
import { Page } from '../components/page/page';
import { SearchBar } from '../../core/components/search-bar/search-bar';
import { UsersTable } from './components/users-table/users-table';
import { TablePagination } from '../../core/components/table-components/table-pagination/table-pagination';
import { UserService } from '../../core/services/user.service';
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/shared/paginated.response';
import UserDto from '../../core/DTOs/user/user.dto';
import UserFields from '../../core/DTOs/user/user-fields.dto';
import { NgIcon } from '@ng-icons/core';
import { UserCreate } from '../../core/components/drawers/user-create/user-create';
import { DrawerService } from '../../core/services/drawer.service';
import { UserEventsService } from '../../core/services/user-events.service';

@Component({
  selector: 'app-users',
  imports: [PageHeader, Page, SearchBar, UsersTable, TablePagination, NgIcon],
  templateUrl: './users.html',
  styleUrl: './users.scss',
})
export class Users implements OnInit {
  headers = ['Email', 'Name', 'Role', 'Active', 'Last Logged In', 'Created Date'];
  users = signal(defaultPaginatedResponse<UserDto>());
  userFields = signal<UserFields>({ roles: [] });

  searchText = signal('');
  showInactive = signal(false);
  pageSize = signal(25);
  pageNumber = signal(1);
  loadingUsers = signal(false);

  constructor(
    private userService: UserService,
    private drawer: DrawerService,
    private userEventsService: UserEventsService
  ) {}

  ngOnInit(): void {
    this.getFields()
    this.getUsers()
    this.userEventsService.usersChanged$.subscribe(() => this.getUsers())
  }

  handleSearch(searchText: string) {
    this.searchText.set(searchText);
    this.getUsers(true);
  }

  handleShowInactive(event: Event) {
    const target = event?.target as HTMLInputElement | null;
    this.showInactive.set(target?.checked ?? false);
    this.getUsers(true);
  }

  handlePaginationChange(pagination: { pageSize: number; pageNumber: number }) {
    this.pageSize.set(pagination.pageSize);
    this.pageNumber.set(pagination.pageNumber);
    this.getUsers();
  }

  getFields() {
    this.userService.getFields().subscribe({
      next: fields => this.userFields.set(fields as UserFields),
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error"),
    });
  }

  getUsers(backToPageOne: boolean = false) {
    if (this.loadingUsers()) return;

    this.loadingUsers.set(true);
    if (backToPageOne) this.pageNumber.set(1)

    this.userService
      .getUsers({
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize(),
        searchText: this.searchText(),
        showInactive: this.showInactive(),
      })
      .subscribe({
        next: users => {
          this.users.set(users as PaginatedResponse<UserDto>);
          this.loadingUsers.set(false);
        },
        error: err => {
          window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error");
          this.users.set(defaultPaginatedResponse<UserDto>());
          this.loadingUsers.set(false);
        },
      });
  }

  openCreateUser() {
    this.drawer.open(UserCreate, {})
  }
}
