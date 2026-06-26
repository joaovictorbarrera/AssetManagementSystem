import { Component, Input } from '@angular/core';

@Component({
  selector: 'thead[app-table-header]',
  imports: [],
  templateUrl: './table-header.html',
  styleUrl: './table-header.scss',
})
export class TableHeader {
  @Input() headers: string[] = []
}
