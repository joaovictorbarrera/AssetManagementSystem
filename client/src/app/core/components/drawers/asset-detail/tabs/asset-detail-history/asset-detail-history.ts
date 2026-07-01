import { Component, Input, OnInit, signal } from '@angular/core';
import { AssetService } from '../../../../../services/api/asset.service';
import AssetHistory from '../../../../../DTOs/asset/asset-history.dto';
import { DatePipe } from '@angular/common';
import { NgIcon } from "@ng-icons/core";
import { SpinningWheel } from "../../../../spinning-wheel/spinning-wheel";

@Component({
  selector: 'app-asset-detail-history',
  imports: [DatePipe, NgIcon, SpinningWheel],
  templateUrl: './asset-detail-history.html',
  styleUrl: './asset-detail-history.scss',
})
export class AssetDetailHistory implements OnInit {
  @Input() assetId!: string

  loading = signal(false)

  history = signal<AssetHistory[]>([])

  constructor(private assetService: AssetService) {}

  ngOnInit(): void {
    this.getHistory()
  }

  getHistory() {
    if (this.loading()) return
    this.loading.set(true)

    this.assetService.getHistory(this.assetId).subscribe({
      next: history => {
        this.history.set(history)
        this.loading.set(false)
      },
      error: err => {
        this.loading.set(false)
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }
}
