import { Component, Input } from '@angular/core';
import { TableHeader } from "../../../../core/components/table-components/table-header/table-header";
import PaginatedResponse from '../../../../core/DTOs/shared/paginated.response';
import { AssetDto } from '../../../../core/DTOs/asset/asset.dto';
import { SpinningWheel } from "../../../../core/components/spinning-wheel/spinning-wheel";
import { AssetRow } from "../asset-row/asset-row";
import { TableWrapper } from "../../../../core/components/table-components/table-wrapper/table-wrapper";
@Component({
  selector: 'app-assets-table',
  imports: [TableHeader, SpinningWheel, AssetRow, TableWrapper],
  templateUrl: './assets-table.html',
  styleUrl: './assets-table.scss',
})
export class AssetsTable {
  @Input() assets!: PaginatedResponse<AssetDto>
  @Input() headers: string[] = []
  @Input() loading: boolean = false
}
