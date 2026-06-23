import { Component } from '@angular/core';
import { PageHeader } from "../components/page-header/page-header";
import { Page } from "../components/page/page";

@Component({
  selector: 'app-users',
  imports: [PageHeader, Page],
  templateUrl: './users.html',
  styleUrl: './users.scss',
})
export class Users {

}
