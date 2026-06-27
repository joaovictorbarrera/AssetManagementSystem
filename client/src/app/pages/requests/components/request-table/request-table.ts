import { Component, Input } from '@angular/core';
import { CheckoutRequest } from '../../../../core/DTOs/checkout-request.dto';
import PaginatedResponse from '../../../../core/DTOs/paginated.response';
import { TableWrapper } from "../../../../core/components/table-components/table-wrapper/table-wrapper";
import { TableHeader } from "../../../../core/components/table-components/table-header/table-header";
import { SpinningWheel } from "../../../../core/components/spinning-wheel/spinning-wheel";
import { RequestRow } from "../request-row/request-row";

@Component({
  selector: 'app-request-table',
  imports: [TableWrapper, TableHeader, SpinningWheel, RequestRow],
  templateUrl: './request-table.html',
  styleUrl: './request-table.scss',
})
export class RequestTable {
  @Input() requests!: PaginatedResponse<CheckoutRequest>
  @Input() headers: string[] = []
  @Input() loading = false
}
