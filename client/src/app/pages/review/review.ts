import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { PageHeader } from "../../core/components/page-components/page-header/page-header";
import { PageWrapper } from "../../core/components/page-components/page-wrapper/page-wrapper";
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { TablePagination } from "../../core/components/table-components/table-pagination/table-pagination";
import { CheckoutRequestDto } from '../../core/DTOs/checkout-request/checkout-request.dto';
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/shared/paginated.response';
import { CheckoutRequestService } from '../../core/services/api/checkout-requests.service';
import CheckoutRequestFields from '../../core/DTOs/checkout-request/checkout-request-fields.dto';
import AssetFields from '../../core/DTOs/asset/asset-fields.dto';
import { AssetService } from '../../core/services/api/asset.service';
import { ReviewTable } from './components/review-table/review-table';
import { NgIcon } from '@ng-icons/core';
import { CheckoutRequestEventsService } from '../../core/services/events/checkout-request-events.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import LabelValuePair from '../../core/DTOs/shared/label-value-pair';
import { toLabelValuePairs } from '../../core/utils/label.utils';
import { Labels } from '../../core/constants/labels';

@Component({
  selector: 'app-review',
  imports: [PageHeader, PageWrapper, Dropdown, ReviewTable, TablePagination, NgIcon],
  templateUrl: './review.html',
  styleUrl: './review.scss',
})
export class Review implements OnInit {
  headers = ['Type', 'Status', 'Asset', 'Category', 'Requestor', 'Request Date']
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

  assetCategoriesList: LabelValuePair[] = []
  requestStatusesList: LabelValuePair[] = []
  requestTypesList: LabelValuePair[] = []

  constructor(
    private requestService: CheckoutRequestService,
    private assetService: AssetService,
    private requestEventsService: CheckoutRequestEventsService,
    private destroyRef: DestroyRef
  ) {}

  ngOnInit(): void {
    this.getFields()
    this.getRequests()
    this.requestEventsService.checkoutRequestsChanged$
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(() => this.getRequests())
  }

  handleStatusChange(status: string) {
    this.status.set(status)
    this.getRequests(true)
  }

  handleTypeChange(type: string) {
    this.type.set(type)
    this.getRequests(true)
  }

  handleCategoryChange(category: string) {
    this.assetCategory.set(category)
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
      next: fields => {
        this.requestFields.set(fields)
        this.requestStatusesList = toLabelValuePairs(fields.statuses, Labels.requestStatuses)
        this.requestTypesList = toLabelValuePairs(fields.types, Labels.requestTypes)
      },
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
    })

    this.assetService.getFields().subscribe({
      next: fields => {
        this.assetFields.set(fields)
        this.assetCategoriesList = toLabelValuePairs(fields.categories, Labels.assetCategories)
      },
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
    })
  }

  getRequests(backToPageOne: boolean = false) {
    if (this.loadingRequests()) return

    this.loadingRequests.set(true)
    if (backToPageOne) this.pageNumber.set(1)

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
        this.requests.set(requests as PaginatedResponse<CheckoutRequestDto>)
        this.loadingRequests.set(false)
      },
      error: err => {
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
        this.loadingRequests.set(false)
      }
    })
  }
}
