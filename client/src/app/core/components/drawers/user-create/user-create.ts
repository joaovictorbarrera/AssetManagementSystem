import { Component, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { SubmitButton } from '../../submit-button/submit-button';
import { UserService } from '../../../services/user.service';
import { DrawerService } from '../../../services/drawer.service';
import UserFields from '../../../DTOs/user/user-fields.dto';
import { Dropdown } from '../../dropdown/dropdown';
import { UserEventsService } from '../../../services/user-events.service';

@Component({
  selector: 'app-user-create',
  imports: [FormsModule, SubmitButton, Dropdown],
  templateUrl: './user-create.html',
  styleUrl: './user-create.scss',
})
export class UserCreate implements OnInit {
  emailAddress = ''
  firstName =  ''
  lastName = ''

  loading = signal(false)
  selectedRole = signal("employee")
  userFields = signal<UserFields>({roles: []})

  constructor(
    private userService: UserService,
    public drawer: DrawerService,
    private userEventsService: UserEventsService
  ) {}

  ngOnInit(): void {
    this.getFields()
  }

  getFields() {
    this.userService.getFields().subscribe({
      next: fields => this.userFields.set(fields as UserFields),
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error"),
    });
  }

  handleDropdownChanged(role: string) {
    this.selectedRole.set(role)
  }

  submit(form: NgForm) {
    if (form.invalid) return;

    this.loading.set(true)
    this.userService.create({
      emailAddress: this.emailAddress,
      firstName: this.firstName,
      lastName: this.lastName,
      role: this.selectedRole()
    }).subscribe({
      next: () => {
        this.loading.set(false)
        this.drawer.close()
        this.userEventsService.emitUsersChanged()
      },
      error: err => {
        this.loading.set(false)
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }
}
