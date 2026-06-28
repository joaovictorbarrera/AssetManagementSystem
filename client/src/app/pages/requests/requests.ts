import { Component, OnInit, signal } from '@angular/core';
import { PageHeader } from "../components/page-header/page-header";
import { Page } from "../components/page/page";
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { RequestTable } from "./components/request-table/request-table";
import { TablePagination } from "../../core/components/table-components/table-pagination/table-pagination";
import { CheckoutRequestDto } from '../../core/DTOs/checkout-request/checkout-request.dto';
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/shared/paginated.response';
import { CheckoutRequestService } from '../../core/services/checkout-requests.service';
import CheckoutRequestFields from '../../core/DTOs/checkout-request/checkout-request-fields.dto';
import AssetFields from '../../core/DTOs/asset/asset-fields.dto';
import { AssetService } from '../../core/services/asset.service';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-requests',
  imports: [PageHeader, Page, Dropdown, RequestTable, TablePagination, NgIcon],
  templateUrl: './requests.html',
  styleUrl: './requests.scss',
})
export class Requests implements OnInit {
  headers = ['Type', 'Status', 'Asset', 'Category', 'Request Date', 'Actions']
  requests = signal(defaultPaginatedResponse<CheckoutRequestDto>())
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
    this.getRequests(true)
  }

  handleTypeChange(type: string) {
    this.type.set(type === "all" ? "" : type)
    this.getRequests(true)
  }

  handleCategoryChange(category: string) {
    this.assetCategory.set(category === "all" ? "" : category)
    this.getRequests(true)
  }

  handleIncludeClosed(event: Event) {
    const target = event?.target as HTMLInputElement | null
    this.includeClosed.set(target?.checked ?? false)
    this.getRequests(true)
  }

  handlePaginationChange(pagination: {pageSize: number, pageNumber: number}) {
    this.pageSize.set(pagination.pageSize)
    this.pageNumber.set(pagination.pageNumber)
    this.getRequests()
  }

  getFields() {
    this.requestService.getFields().subscribe({
      next: fields => this.requestFields.set(fields as CheckoutRequestFields),
      error: err => window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
    })

    this.assetService.getFields().subscribe({
      next: fields => this.assetFields.set(fields as AssetFields),
      error: err => window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
    })
  }

  getRequests(backToPageOne: boolean = false) {
    if (this.loadingRequests()) return
    if (backToPageOne) this.pageNumber.set(1)

    this.loadingRequests.set(true)
    this.requestService.getCheckoutRequests({
      type: this.type(),
      assetCategory: this.assetCategory(),
      status: this.status(),
      includeClosedRequests: this.includeClosed(),
      pageSize: this.pageSize(),
      pageNumber: this.pageNumber()
    }).subscribe({
      next: requests => {
        this.requests.set(requests as PaginatedResponse<CheckoutRequestDto>)
        this.loadingRequests.set(false)
      },
      error: err => {
        window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
        this.loadingRequests.set(false)
      }
    })
  }
}
