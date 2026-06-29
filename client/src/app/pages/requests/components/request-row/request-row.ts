import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CheckoutRequestDto } from '../../../../core/DTOs/checkout-request/checkout-request.dto';
import { DatePipe } from '@angular/common';
import { CheckoutRequestService } from '../../../../core/services/checkout-requests.service';

@Component({
  selector: 'tr[app-request-row]',
  imports: [DatePipe],
  templateUrl: './request-row.html',
  styleUrl: './request-row.scss',
})
export class RequestRow {
  @Input() request!: CheckoutRequestDto
  @Output() cancelled = new EventEmitter<string>();

  constructor(private requestService: CheckoutRequestService) {}

  handleCancel() {
    if (window.confirm("Are you sure you want to cancel this request?")) {
      this.requestService.cancel(this.request.id).subscribe({
        next: () => this.cancelled.emit(this.request.id),
        error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      })
    }
  }
}


