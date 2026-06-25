import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { Page } from "../components/page/page";
import { PageHeader } from '../components/page-header/page-header';
import { AssetService } from '../../core/services/asset.service';
import AssetFields from '../../core/DTOs/asset-fields.dto';
import { Dropdown } from "../../core/components/dropdown/dropdown";
import { SearchBar } from "../../core/components/search-bar/search-bar";

@Component({
  selector: 'app-dashboard',
  imports: [Page, PageHeader, Dropdown, SearchBar],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  assetFields: WritableSignal<AssetFields> = signal({categories: [], statuses: [], conditions: []})
  category = signal("")
  status = signal("")
  condition = signal("")
  searchText = signal("")
  assets = []
  pageSize = 50
  pageNumber = 1

  constructor(private assetService: AssetService) {}

  ngOnInit(): void {
    this.assetService.getFields().subscribe({
      next: res => this.assetFields.set(res as AssetFields)
    })
  }

  handleSearch(searchText: string) {
    this.searchText.set(searchText)
    this.getAssets()
  }

  handleConditionChange(value: string) {
    this.condition.set(value === "all" ? "" : value)
    this.getAssets()
  }

  handleCategoryChange(value: string) {
    this.category.set(value === "all" ? "" : value)
    this.getAssets()
  }

  handleStatusChange(value: string) {
    this.status.set(value === "all" ? "" : value)
    this.getAssets()
  }

  getAssets() {
    this.assetService.getAssets({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchText: this.searchText(),
      condition: this.condition(),
      status: this.status(),
      category: this.category()
    }).subscribe()
  }
}
