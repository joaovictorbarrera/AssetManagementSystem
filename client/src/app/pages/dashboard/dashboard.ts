import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { Page } from "../components/page/page";
import { PageHeader } from '../components/page-header/page-header';
import { AssetService } from '../../core/services/asset.service';
import AssetFields from '../../core/DTOs/asset-fields.dto';
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { SearchBar } from "../../core/components/search-bar/search-bar";
import { AssetsTable } from "./components/assets-table/assets-table";
import PaginatedResponse, { defaultPaginatedResponse } from '../../core/DTOs/paginated.response';
import { Asset } from '../../core/DTOs/asset.dto';
import { TablePagination } from "../../core/components/table-components/table-pagination/table-pagination";

@Component({
  selector: 'app-dashboard',
  imports: [Page, PageHeader, Dropdown, SearchBar, AssetsTable, TablePagination],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  assetFields: WritableSignal<AssetFields> = signal({categories: [], statuses: [], conditions: []})
  headers = ["Asset Tag", "Name", "Category", "Status", "Return Status"]

  category = signal("")
  status = signal("")
  searchText = signal("")
  assets = signal(defaultPaginatedResponse<Asset>())
  pageSize = signal(25)
  pageNumber = signal(1)

  loadingAssets = signal(false)

  constructor(private assetService: AssetService) {}

  ngOnInit(): void {
    this.getFields()
    this.getAssets()
  }

  handleSearch(searchText: string) {
    this.searchText.set(searchText)
    this.getAssets()
  }

  handleStatusChange(value: string) {
    this.status.set(value === "all" ? "" : value)
    this.getAssets()
  }

  handleCategoryChange(value: string) {
    this.category.set(value === "all" ? "" : value)
    this.getAssets()
  }

  handlePaginationChange(pagination: { pageNumber: number; pageSize: number }) {
    this.pageNumber.set(pagination.pageNumber)
    this.pageSize.set(pagination.pageSize)
    this.getAssets()
  }

  getFields() {
    this.assetService.getFields().subscribe({
      next: res => this.assetFields.set(res as AssetFields),
      error: err => window.alert(err.message)
    })
  }

  getAssets() {
    if (this.loadingAssets()) return

    this.loadingAssets.set(true)
    this.assetService.getAssets({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
      searchText: this.searchText(),
      status: this.status(),
      category: this.category()
    }).subscribe({
      next: data => {
        this.assets.set(data as PaginatedResponse<Asset>)
        this.loadingAssets.set(false)
      },
      error: (err) => {
        window.alert(err.message)
        this.assets.set(defaultPaginatedResponse<Asset>())
        this.loadingAssets.set(false)
      }
    })
  }
}
