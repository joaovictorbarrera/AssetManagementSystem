import { Component, Input, OnInit, signal } from '@angular/core';
import { AssetDetailDto } from '../../../DTOs/asset/asset-detail.dto';
import { FormsModule } from '@angular/forms';
import { AssetDetailForm } from './pages/asset-detail-form/asset-detail-form';
import { SpinningWheel } from "../../spinning-wheel/spinning-wheel";
import { AssetService } from '../../../services/api/asset.service';
import { AuthService } from '../../../services/api/auth.service';
import { DrawerService } from '../../../services/util/drawer.service';
import { AssetDetailHistory } from './pages/asset-detail-history/asset-detail-history';

@Component({
  selector: 'app-asset-detail',
  imports: [FormsModule, AssetDetailForm, SpinningWheel, AssetDetailHistory],
  templateUrl: './asset-detail.html',
  styleUrl: './asset-detail.scss',
})
export class AssetDetail implements OnInit {
  @Input() assetId!: string

  page = signal('details')

  assetDetails = signal<AssetDetailDto | null>(null)
  loadingDetails = signal(true)

  constructor(
    private assetService: AssetService,
    public authService: AuthService,
    public drawer: DrawerService
  ) {}

  ngOnInit(): void {
    this.getDetail()
  }

  handleOpenDetails() { this.page.set('details') }
  handleOpenHistory() { this.page.set('history') }

  getDetail() {
    this.assetService.getDetail(this.assetId).subscribe({
      next: data => {
        this.assetDetails.set(data)
        this.loadingDetails.set(false)
      },
      error: err => {
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }
}
