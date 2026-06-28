import { Component, Input, signal, ViewChild } from '@angular/core';
import { AssetDto } from '../../../../core/DTOs/asset/asset.dto';
import { CheckoutRequestService } from '../../../../core/services/checkout-requests.service';
import { Dropdown } from '../../../../core/components/dropdown/dropdown';
import AssetFields from '../../../../core/DTOs/asset/asset-fields.dto';
import { AssetService } from '../../../../core/services/asset.service';

@Component({
  selector: 'tr[app-inventory-row]',
  imports: [Dropdown],
  templateUrl: './inventory-row.html',
  styleUrl: './inventory-row.scss',
})
export class InventoryRow {
  @Input() asset!: AssetDto
  @Input() assetFields!: AssetFields
  @ViewChild('statusDropdown') statusDropdown!: Dropdown

  showStatusSuccess = signal(false)
  showConditionSuccess = signal(false)

  constructor(private assetService: AssetService) {}

  handleStatusChange(status: string) {
    if (status === 'available' && this.asset.userId) {
      const confirmed = window.confirm(
        'Making an asset available will unassign it from the user. Do you want to continue?'
      )
      if (!confirmed) {
        this.statusDropdown.revert()
        return
      }
    }

    this.assetService.updateStatus(this.asset.id, status).subscribe({
        next: () => {
          if (status === 'available') {
            this.asset.userId = undefined
            this.asset.userFirstName = undefined
            this.asset.userLastName = undefined
          }
          this.asset.status = status
          this.showStatusSuccess.set(true)
          setTimeout(() => this.showStatusSuccess.set(false), 1500)
        },
        error: err => {
          window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
        }
    })
  }

  handleConditionChange(condition: string) {
    this.assetService.updateCondition(this.asset.id, condition).subscribe({
        next: () => {
          this.asset.condition = condition
          this.showConditionSuccess.set(true)
          setTimeout(() => this.showConditionSuccess.set(false), 1500)
        },
        error: err => {
          window.alert(`${err.status} error: ` + err.error.message ? err.error.message : "Unknown Error")
        }
    })
  }
}
