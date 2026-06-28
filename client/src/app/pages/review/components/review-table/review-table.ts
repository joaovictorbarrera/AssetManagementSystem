import { Component, Input } from '@angular/core';
import PaginatedResponse from '../../../../core/DTOs/paginated.response';
import { CheckoutRequestDto } from '../../../../core/DTOs/checkout-request.dto';
import { ReviewRow } from '../review-row/review-row';
import { TableWrapper } from '../../../../core/components/table-components/table-wrapper/table-wrapper';
import { SpinningWheel } from '../../../../core/components/spinning-wheel/spinning-wheel';
import { TableHeader } from '../../../../core/components/table-components/table-header/table-header';

@Component({
  selector: 'app-review-table',
  imports: [ReviewRow, TableWrapper, SpinningWheel, TableHeader],
  templateUrl: './review-table.html',
  styleUrl: './review-table.scss',
})
export class ReviewTable {
  @Input() requests!: PaginatedResponse<CheckoutRequestDto>
  @Input() headers: string[] = []
  @Input() loading = false
}
