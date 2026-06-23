import { Component } from '@angular/core';
import { Page } from "../components/page/page";
import { PageHeader } from '../components/page-header/page-header';

@Component({
  selector: 'app-dashboard',
  imports: [Page, PageHeader],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard {

}
