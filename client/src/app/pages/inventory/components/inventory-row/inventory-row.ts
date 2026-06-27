import { Component, Input, ViewChild } from '@angular/core';
import { Asset } from '../../../../core/DTOs/asset.dto';
import { CheckoutRequestService } from '../../../../core/services/checkout-requests.service';
import { Dropdown } from '../../../../core/components/dropdown/dropdown';
import AssetFields from '../../../../core/DTOs/asset-fields.dto';

@Component({
  selector: 'tr[app-inventory-row]',
  imports: [Dropdown],
  templateUrl: './inventory-row.html',
  styleUrl: './inventory-row.scss',
})
export class InventoryRow {
  @Input() asset!: Asset
  @Input() assetFields!: AssetFields
  @ViewChild('statusDropdown') statusDropdown!: Dropdown

  constructor(private requestService: CheckoutRequestService) {}

  handleStatusChange(status: string) {
    if (status === 'available' && this.asset.assignedToUser) {
      const confirmed = window.confirm(
        'Making an asset available will unassign it from the user. Do you want to continue?'
      )
      if (!confirmed) {
        this.statusDropdown.revert()
        return
      }
      this.asset.assignedToUser = null
    }
    this.asset.status = status
  }
}