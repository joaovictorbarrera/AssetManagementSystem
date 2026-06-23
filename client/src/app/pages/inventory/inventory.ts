import { Component } from '@angular/core';
import { PageHeader } from "../components/page-header/page-header";
import { Page } from "../components/page/page";

@Component({
  selector: 'app-inventory',
  imports: [PageHeader, Page],
  templateUrl: './inventory.html',
  styleUrl: './inventory.scss',
})
export class Inventory {

}
