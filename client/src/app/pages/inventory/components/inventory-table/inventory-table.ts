import { Component, Input } from '@angular/core';
import { TableHeader } from "../../../../core/components/table-components/table-header/table-header";
import PaginatedResponse from '../../../../core/DTOs/shared/paginated.response';
import { AssetDto } from '../../../../core/DTOs/asset/asset.dto';
import { SpinningWheel } from "../../../../core/components/spinning-wheel/spinning-wheel";
import { InventoryRow } from "../inventory-row/inventory-row";
import { TableWrapper } from "../../../../core/components/table-components/table-wrapper/table-wrapper";
import AssetFields from '../../../../core/DTOs/asset/asset-fields.dto';

@Component({
  selector: 'app-inventory-table',
  imports: [TableHeader, SpinningWheel, InventoryRow, TableWrapper],
  templateUrl: './inventory-table.html',
  styleUrl: './inventory-table.scss',
})
export class InventoryTable {
  @Input() assets!: PaginatedResponse<AssetDto>
  @Input() headers: string[] = []
  @Input() loading: boolean = false
  @Input() assetFields!: AssetFields
}
