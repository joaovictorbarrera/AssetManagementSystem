import { Component, OnInit, signal } from '@angular/core';
import { DrawerService } from '../../../services/util/drawer.service';
import { CheckoutRequestService } from '../../../services/api/checkout-requests.service';
import { AssetService } from '../../../services/api/asset.service';
import AssetFields from '../../../DTOs/asset/asset-fields.dto';
import { FormsModule, NgForm } from '@angular/forms';
import { SubmitButton } from '../../submit-button/submit-button';
import { Dropdown } from '../../dropdown/dropdown';
import { CheckoutRequestEventsService } from '../../../services/events/checkout-request-events.service';
import LabelValuePair from '../../../DTOs/shared/label-value-pair';
import { toLabelValuePairs } from '../../../utils/label.utils';
import { Labels } from '../../../constants/labels';

@Component({
  selector: 'app-request-create',
  imports: [FormsModule, SubmitButton, Dropdown],
  templateUrl: './request-create.html',
  styleUrl: './request-create.scss',
})
export class RequestCreate implements OnInit {

  category = signal("laptop")
  reason = ""

  loading = signal(false)
  assetCategoryList: LabelValuePair[] = []

  constructor(
    public drawer: DrawerService,
    private requestService: CheckoutRequestService,
    private assetService: AssetService,
    private requestEventsService: CheckoutRequestEventsService
  ) {}

  ngOnInit(): void {
    this.getFields()
  }

  handleCategoryChange(category: string) {
    this.category.set(category)
  }

  getFields() {
    this.assetService.getFields().subscribe({
      next: fields => {
        this.assetCategoryList = toLabelValuePairs(fields.categories, Labels.assetCategories)
      },
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
    })
  }

  submit(form: NgForm) {
    if (form.invalid) return
    console.log(this.category())
    this.loading.set(true)
    this.requestService.create({
      requestType: 'checkout',
      reason: this.reason,
      assetCategory: this.category()
    }).subscribe({
      next: () => {
        this.loading.set(false)
        this.drawer.close()
        this.requestEventsService.emitCheckoutRequestsChanged()
      },
      error: err => {
        this.loading.set(false)
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }
}
