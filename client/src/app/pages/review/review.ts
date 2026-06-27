import { Component, OnInit, signal } from '@angular/core';
import { PageHeader } from "../components/page-header/page-header";
import { Page } from "../components/page/page";
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { TablePagination } from "../../core/components/table-components/table-pagination/table-pagination";
import { CheckoutRequest } from '../../core/DTOs/checkout-request.dto';
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/paginated.response';
import { CheckoutRequestService } from '../../core/services/checkout-requests.service';
import CheckoutRequestFields from '../../core/DTOs/checkout-request-fields.dto';
import AssetFields from '../../core/DTOs/asset-fields.dto';
import { AssetService } from '../../core/services/asset.service';
import { RequestTable } from '../requests/components/request-table/request-table';

@Component({
  selector: 'app-review',
  imports: [PageHeader, Page, Dropdown, RequestTable, TablePagination],
  templateUrl: './review.html',
  styleUrl: './review.scss',
})
export class Review implements OnInit {
  headers = ['Type', 'Status', 'Asset', 'Category', 'Request Date', 'Actions']
  requests = signal(defaultPaginatedResponse<CheckoutRequest>())
  requestFields = signal<CheckoutRequestFields>({types: [], statuses: []})
  assetFields = signal<AssetFields>({categories: [], statuses: [], conditions: []})

  type = signal("")
  status = signal("")
  assetCategory = signal("")
  includeClosed = signal(false)
  pageSize = signal(25)
  pageNumber = signal(1)

  loadingRequests = signal(false)

  constructor(
    private requestService: CheckoutRequestService,
    private assetService: AssetService
  ) {}

  ngOnInit(): void {
    this.getFields()
    this.getRequests()
  }

  handleStatusChange(status: string) {
    this.status.set(status === "all" ? "" : status)
    this.getRequests()
  }

  handleTypeChange(type: string) {
    this.type.set(type === "all" ? "" : type)
    this.getRequests()
  }

  handleCategoryChange(category: string) {
    this.assetCategory.set(category === "all" ? "" : category)
    this.getRequests()
  }

  handleIncludeClosed(event: Event) {
    const target = event?.target as HTMLInputElement | null
    this.includeClosed.set(target?.checked ?? false)
    this.getRequests()
  }

  handlePaginationChange(pagination: {pageSize: number, pageNumber: number}) {
    this.pageSize.set(pagination.pageSize)
    this.pageNumber.set(pagination.pageNumber)
    this.getRequests()
  }

  getFields() {
    this.requestService.getFields().subscribe({
      next: fields => this.requestFields.set(fields as CheckoutRequestFields),
      error: err => window.alert(err.message)
    })

    this.assetService.getFields().subscribe({
      next: fields => this.assetFields.set(fields as AssetFields),
      error: err => window.alert(err.message)
    })
  }

  getRequests() {
    this.loadingRequests.set(true)
    this.requestService.getCheckoutRequests({
      type: this.type(),
      assetCategory: this.assetCategory(),
      status: this.status(),
      includeClosedRequests: this.includeClosed(),
      pageSize: this.pageSize(),
      pageNumber: this.pageNumber(),
      review: true
    }).subscribe({
      next: requests => {
        this.requests.set(requests as PaginatedResponse<CheckoutRequest>)
        this.loadingRequests.set(false)
      },
      error: err => {
        window.alert(err.message)
        this.loadingRequests.set(false)
      }
    })
  }
}
