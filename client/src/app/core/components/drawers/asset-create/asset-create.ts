import { Component, OnInit, signal } from '@angular/core';
import { DrawerService } from '../../../services/util/drawer.service';
import { AssetService } from '../../../services/api/asset.service';
import AssetFields from '../../../DTOs/asset/asset-fields.dto';
import { FormsModule, NgForm } from '@angular/forms';
import { SubmitButton } from '../../submit-button/submit-button';
import { Dropdown } from '../../dropdown/dropdown';
import { AssetEventsService } from '../../../services/events/asset-events.service';
import { Labels } from '../../../constants/labels';
import { toLabelValuePairs } from '../../../utils/label.utils';
import LabelValuePair from '../../../DTOs/shared/label-value-pair';

@Component({
  selector: 'app-asset-create',
  imports: [FormsModule, SubmitButton, Dropdown],
  templateUrl: './asset-create.html',
  styleUrl: './asset-create.scss',
})
export class AssetCreate implements OnInit {

  assetName = ''
  assetTag = ''
  serialNumber = ''
  category = signal('')
  condition = signal('')

  loading = signal(false)
  assetFields = signal<AssetFields>({categories: [], statuses: [], conditions: []})

  assetCategoriesList: LabelValuePair[] = []
  assetConditionsList: LabelValuePair[] = []

  constructor(
    public drawer: DrawerService,
    private assetService: AssetService,
    private assetEventsService: AssetEventsService
  ) {}

  ngOnInit(): void {
    this.getFields()
  }

  handleCategoryChange(category: string) {
    this.category.set(category)
  }

  handleConditionChange(condition: string) {
    this.condition.set(condition)
  }

  getFields() {
    this.assetService.getFields().subscribe({
      next: res => {
        this.assetFields.set(res)
        this.assetCategoriesList = toLabelValuePairs(this.assetFields().categories, Labels.assetCategories)
        this.assetConditionsList = toLabelValuePairs(this.assetFields().conditions, Labels.assetConditions)
      },
      error: err => window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
    })
  }

  submit(form: NgForm) {
    if (form.invalid) return
    this.loading.set(true)
    this.assetService.create({
        assetTag: this.assetTag,
        name: this.assetName,
        serialNumber: this.serialNumber,
        category: this.category(),
        condition: this.condition()
      }).subscribe({
      next: () => {
        this.loading.set(false)
        this.drawer.close()
        this.assetEventsService.emitAssetsChanged()
      },
      error: err => {
        this.loading.set(false)
        window.alert(`${err.status} error: ` + err.error.title ? err.error.title : "Unknown Error")
      }
    })
  }
}
