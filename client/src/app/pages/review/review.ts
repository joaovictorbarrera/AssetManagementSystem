import { Component } from '@angular/core';
import { PageHeader } from "../components/page-header/page-header";
import { Page } from "../components/page/page";

@Component({
  selector: 'app-review',
  imports: [PageHeader, Page],
  templateUrl: './review.html',
  styleUrl: './review.scss',
})
export class Review {

}
