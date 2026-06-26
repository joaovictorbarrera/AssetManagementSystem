import { Component, Input } from '@angular/core';
import { TableHeader } from "../../../../core/components/table-components/table-header/table-header";
import { TablePagination } from "../../../../core/components/table-components/table-pagination/table-pagination";
import PaginatedResponse from '../../../../core/DTOs/paginated.response';
import { Asset } from '../../../../core/DTOs/asset.dto';
import { SpinningWheel } from "../../../../core/components/spinning-wheel/spinning-wheel";

@Component({
  selector: 'app-assets-table',
  imports: [TableHeader, SpinningWheel],
  templateUrl: './assets-table.html',
  styleUrl: './assets-table.scss',
})
export class AssetsTable {
  @Input() assets!: PaginatedResponse<Asset>
  @Input() headers: string[] = []
  @Input() loading: boolean = false
}
