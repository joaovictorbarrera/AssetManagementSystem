import { Component, Input, OnInit, signal } from '@angular/core';
import { AssetDetailDto } from '../../../../../DTOs/asset/asset-detail.dto';
import { DatePipe } from '@angular/common';
import { SpinningWheel } from '../../../../spinning-wheel/spinning-wheel';
import { SubmitButton } from '../../../../submit-button/submit-button';
import { FormsModule, NgForm } from '@angular/forms';
import AssetFields from '../../../../../DTOs/asset/asset-fields.dto';
import { DrawerService } from '../../../../../services/util/drawer.service';
import { AssetEventsService } from '../../../../../services/events/asset-events.service';
import { AuthService } from '../../../../../services/api/auth.service';
import { AssetService } from '../../../../../services/api/asset.service';

@Component({
  selector: 'app-asset-detail-form',
  imports: [DatePipe, SubmitButton, FormsModule],
  templateUrl: './asset-detail-form.html',
  styleUrl: './asset-detail-form.scss',
})
export class AssetDetailForm implements OnInit {
  @Input() assetDetails!: AssetDetailDto

  loadingUpdate = signal(false)
  loadingArchive = signal(false)

  assetName = ''
  assetTag = ''
  serialNumber = ''

  constructor(
    private assetService: AssetService,
    public authService: AuthService,
    private assetEventsService: AssetEventsService,
    public drawer: DrawerService
  ) {}

  ngOnInit(): void {
    this.assetName = this.assetDetails.name
    this.assetTag = this.assetDetails.assetTag
    this.serialNumber = this.assetDetails.serialNumber
  }

  archive() {
    if (this.loadingArchive()) return

    this.loadingArchive.set(true)
    this.assetService.archive(this.assetDetails.id).subscribe({
      next: () => {
        this.loadingArchive.set(false)
        this.drawer.close()
        this.assetEventsService.emitAssetsChanged()
      },
      error: err => {
        this.loadingArchive.set(false)
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }

  submit(ngForm: NgForm) {
    if (ngForm.invalid || this.loadingUpdate()) return

    this.loadingUpdate.set(true)
    this.assetService.update(this.assetDetails.id, {
      assetTag: this.assetTag,
      name: this.assetName,
      serialNumber: this.serialNumber
    }).subscribe({
      next: () => {
        this.loadingUpdate.set(false)
        this.drawer.close()
        this.assetEventsService.emitAssetsChanged()
      },
      error: err => {
        this.loadingUpdate.set(false)
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }
}
