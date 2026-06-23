import { Component } from '@angular/core';
import { PageHeader } from "../components/page-header/page-header";
import { Page } from "../components/page/page";

@Component({
  selector: 'app-requests',
  imports: [PageHeader, Page],
  templateUrl: './requests.html',
  styleUrl: './requests.scss',
})
export class Requests {

}
