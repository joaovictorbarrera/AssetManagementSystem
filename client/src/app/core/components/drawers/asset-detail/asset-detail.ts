import { Component, EventEmitter, Input, OnInit, Output, signal } from '@angular/core';
import { AssetService } from '../../../services/asset.service';
import { AssetDetailDto } from '../../../DTOs/asset/asset-detail.dto';
import { SpinningWheel } from '../../spinning-wheel/spinning-wheel';
import { AuthService } from '../../../services/auth.service';
import { DatePipe } from '@angular/common';
import { Dropdown } from '../../dropdown/dropdown';
import AssetFields from '../../../DTOs/asset/asset-fields.dto';
import { DrawerService } from '../../../services/drawer.service';
import { AssetEventsService } from '../../../services/asset-events.service';

@Component({
  selector: 'app-asset-detail',
  imports: [SpinningWheel, DatePipe, Dropdown],
  templateUrl: './asset-detail.html',
  styleUrl: './asset-detail.scss',
})
export class AssetDetail implements OnInit {
  @Input() assetId!: string

  assetFields = signal<AssetFields>({categories: [], statuses: [], conditions: []})
  assetDetails = signal<AssetDetailDto | null>(null)
  loadingDetails = signal(true)

  constructor(
    private assetService: AssetService,
    public authService: AuthService,
    private assetEvents: AssetEventsService,
    private drawerService: DrawerService
  ) {}

  ngOnInit(): void {
    this.getFields()
    this.getDetail()
  }

  getDetail() {
    this.assetService.getDetail(this.assetId).subscribe({
      next: data => {
        this.assetDetails.set(data)
        this.loadingDetails.set(false)
      },
      error: err => {
        window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
      }
    })
  }

  getFields() {
    this.assetService.getFields().subscribe({
      next: res => this.assetFields.set(res as AssetFields),
      error: err => window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
    })
  }

  handleArchive() {
    this.assetService.archive(this.assetId).subscribe({
      next: () => {
        this.drawerService.close()
        this.assetEvents.emitAssetUpdated()
      },
      error: err => window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
    })
  }
}
