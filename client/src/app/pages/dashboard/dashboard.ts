import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { PageWrapper } from "../../core/components/page-components/page-wrapper/page-wrapper";
import { PageHeader } from '../../core/components/page-components/page-header/page-header';
import { AssetService } from '../../core/services/api/asset.service';
import AssetFields from '../../core/DTOs/asset/asset-fields.dto';
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { SearchBar } from "../../core/components/search-bar/search-bar";
import { AssetsTable } from "./components/assets-table/assets-table";
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/shared/paginated.response';
import { AssetDto } from '../../core/DTOs/asset/asset.dto';
import { TablePagination } from "../../core/components/table-components/table-pagination/table-pagination";
import { NgIcon } from '@ng-icons/core';
import { AssetEventsService } from '../../core/services/events/asset-events.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toLabelValuePairs } from '../../core/utils/label.utils';
import { Labels } from '../../core/constants/labels';
import LabelValuePair from '../../core/DTOs/shared/label-value-pair';

@Component({
  selector: 'app-dashboard',
  imports: [PageWrapper, PageHeader, Dropdown, SearchBar, AssetsTable, TablePagination, NgIcon],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  assetFields = signal<AssetFields>({categories: [], statuses: [], conditions: []})
  headers = ["Asset Tag", "Name", "Category", "Status", "Return Status"]

  category = signal("")
  status = signal("")
  searchText = signal("")
  assets = signal(defaultPaginatedResponse<AssetDto>())
  pageSize = signal(25)
  pageNumber = signal(1)

  loadingAssets = signal(false)

  constructor(
    private assetService: AssetService,
    private assetEvents: AssetEventsService,
    private destroyRef: DestroyRef
  ) {}

  assetCategoriesList: LabelValuePair[] = []

  ngOnInit(): void {
    this.getFields()
    this.getAssets()
    this.assetEvents.assetsChanged$
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(() => this.getAssets());
  }

  handleSearch(searchText: string) {
    this.searchText.set(searchText)
    this.getAssets(true)
  }

  handleStatusChange(status: string) {
    this.status.set(status)
    this.getAssets(true)
  }

  handleCategoryChange(category: string) {
    this.category.set(category)
    this.getAssets(true)
  }

  handlePaginationChange(pagination: { pageNumber: number; pageSize: number }) {
    this.pageNumber.set(pagination.pageNumber)
    this.pageSize.set(pagination.pageSize)
    this.getAssets()
  }

  getFields() {
    this.assetService.getFields().subscribe({
      next: res => {
        this.assetFields.set(res)
        this.assetCategoriesList = toLabelValuePairs(this.assetFields().categories, Labels.assetCategories)
      },
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
    })
  }

  getAssets(backToPageOne: boolean = false) {
    if (this.loadingAssets()) return

    if (backToPageOne) this.pageNumber.set(1)

    this.loadingAssets.set(true)
    this.assetService.getAssets({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
      searchText: this.searchText(),
      status: this.status(),
      category: this.category()
    }).subscribe({
      next: data => {
        this.assets.set(data as PaginatedResponse<AssetDto>)
        this.loadingAssets.set(false)
      },
      error: (err) => {
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
        this.assets.set(defaultPaginatedResponse<AssetDto>())
        this.loadingAssets.set(false)
      }
    })
  }
}
