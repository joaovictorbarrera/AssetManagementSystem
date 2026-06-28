import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { Page } from "../components/page/page";
import { PageHeader } from '../components/page-header/page-header';
import { AssetService } from '../../core/services/asset.service';
import AssetFields from '../../core/DTOs/asset-fields.dto';
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { SearchBar } from "../../core/components/search-bar/search-bar";
import { InventoryTable } from "./components/inventory-table/inventory-table";
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/paginated.response';
import { AssetDto } from '../../core/DTOs/asset.dto';
import { TablePagination } from "../../core/components/table-components/table-pagination/table-pagination";
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-inventory',
  imports: [Page, PageHeader, Dropdown, SearchBar, InventoryTable, TablePagination, NgIcon],
  templateUrl: './inventory.html',
  styleUrl: './inventory.scss',
})
export class Inventory implements OnInit {
  assetFields: WritableSignal<AssetFields> = signal({categories: [], statuses: [], conditions: []})
  headers = ["Asset Tag", "Name", "Category", "Status", "Assigned To", "Condition"]
  assets = signal(defaultPaginatedResponse<AssetDto>())

  category = signal("")
  status = signal("")
  condition = signal("")

  searchText = signal("")
  includeArchived = signal(false)

  pageSize = signal(25)
  pageNumber = signal(1)

  loadingAssets = signal(false)

  constructor(private assetService: AssetService) {}

  ngOnInit(): void {
    this.getFields()
    this.getAssets()
  }

  handleIncludeArchived(event: Event) {
    const target = event?.target as HTMLInputElement | null
    this.includeArchived.set(target?.checked ?? false)
    this.getAssets(true)
  }

  handleSearch(searchText: string) {
    this.searchText.set(searchText)
    this.getAssets(true)
  }

  handleStatusChange(status: string) {
    this.status.set(status === "all" ? "" : status)
    this.getAssets(true)
  }

  handleCategoryChange(category: string) {
    this.category.set(category === "all" ? "" : category)
    this.getAssets(true)
  }

  handleConditionChange(condition: string) {
    this.condition.set(condition === "all" ? "" : condition)
    this.getAssets(true)
  }

  handlePaginationChange(pagination: { pageNumber: number; pageSize: number }) {
    this.pageNumber.set(pagination.pageNumber)
    this.pageSize.set(pagination.pageSize)
    this.getAssets()
  }

  getFields() {
    this.assetService.getFields().subscribe({
      next: res => this.assetFields.set(res as AssetFields),
      error: err => window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
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
      condition: this.condition(),
      category: this.category(),
      inventory: true,
      includeArchived: this.includeArchived()
    }).subscribe({
      next: data => {
        this.assets.set(data as PaginatedResponse<AssetDto>)
        this.loadingAssets.set(false)
      },
      error: (err) => {
        window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
        this.assets.set(defaultPaginatedResponse<AssetDto>())
        this.loadingAssets.set(false)
      }
    })
  }
}
