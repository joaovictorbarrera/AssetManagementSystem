import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, signal } from '@angular/core';
import Pagination from '../../../DTOs/shared/pagination';
import { FormsModule } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'app-table-pagination',
  imports: [FormsModule, NgIcon],
  templateUrl: './table-pagination.html',
  styleUrl: './table-pagination.scss',
})
export class TablePagination implements OnChanges {
  @Input() colspan!: number
  @Input() pagination!: Pagination
  @Input() name!: string
  @Output() paginationChanged = new EventEmitter<{ pageSize: number; pageNumber: number }>();

  pageNumber = 1
  pageSize = 25

  ngOnChanges() {
    if (!this.pagination) return;

    this.pageNumber = this.pagination.pageNumber;
    this.pageSize = this.pagination.pageSize;
  }

  get showingFrom() {
    if (!this.pagination || this.pagination.totalCount === 0) {
      return 0
    }

    return Math.min(
      this.pagination.pageSize * (this.pageNumber - 1) + 1,
      this.pagination.totalCount
    )
  }

  get showingTo() {
    if (!this.pagination || this.pagination.totalCount === 0) {
      return 0
    }

    return Math.min(
      this.pagination.pageSize * this.pageNumber,
      this.pagination.totalCount
    )
  }

  handleNextPage() {
    this.pageNumber++
    this.emitPaginationChanged()
  }

  handlePreviousPage() {
    this.pageNumber--
    this.emitPaginationChanged()
  }

  emitPaginationChanged(backToPageOne: boolean = false) {
    this.paginationChanged.emit({
      pageSize: this.pageSize,
      pageNumber: backToPageOne ? 1 : this.pageNumber
    })
  }
}
