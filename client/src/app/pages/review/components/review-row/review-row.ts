import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { CheckoutRequestDto } from '../../../../core/DTOs/checkout-request.dto';

@Component({
  selector: 'tr[app-review-row]',
  imports: [DatePipe],
  templateUrl: './review-row.html',
  styleUrl: './review-row.scss',
})
export class ReviewRow {
  @Input() request!: CheckoutRequestDto
}
