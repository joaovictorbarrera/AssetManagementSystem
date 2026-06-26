import { Component, Input } from '@angular/core';
import { Asset } from '../../../../core/DTOs/asset.dto';
import { CheckoutRequestService } from '../../../../core/services/checkout-requests.service';

@Component({
  selector: 'tr[app-asset-row]',
  imports: [],
  templateUrl: './asset-row.html',
  styleUrl: './asset-row.scss',
})
export class AssetRow {
  @Input() asset!: Asset

  constructor(private requestService: CheckoutRequestService) {}

  handleReturnRequest() {
    if (window.confirm("Are you sure you want to return this asset?")) {
      let reason = window.prompt("What is the reason for return?")
      this.asset.isPendingReturn = true
      this.requestService.create({
        requestType: "return",
        assetCategory: this.asset.category,
        assetId: this.asset.id,
        reason
      }).subscribe({

      })
    }
  }
}
