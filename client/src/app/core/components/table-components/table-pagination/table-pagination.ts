import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, signal } from '@angular/core';
import Pagination from '../../../DTOs/pagination';
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

  pageNumber = signal(1)
  pageSize = 25

  ngOnChanges(changes: SimpleChanges) {
    if (changes['pagination'] && this.pagination) {
      this.pageNumber.set(this.pagination.pageNumber || 1)
      this.pageSize = this.pagination.pageSize || 25
    }
  }

  get showingFrom() {
    if (!this.pagination || this.pagination.totalCount === 0) {
      return 0
    }

    return Math.min(
      this.pagination.pageSize * (this.pageNumber() - 1) + 1,
      this.pagination.totalCount
    )
  }

  get showingTo() {
    if (!this.pagination || this.pagination.totalCount === 0) {
      return 0
    }

    return Math.min(
      this.pagination.pageSize * this.pageNumber(),
      this.pagination.totalCount
    )
  }

  handleNextPage() {
    this.pageNumber.set(this.pageNumber() + 1)
    this.emitPaginationChanged()
  }

  handlePreviousPage() {
    this.pageNumber.set(this.pageNumber() - 1)
    this.emitPaginationChanged()
  }

  emitPaginationChanged(backToPageOne: boolean = false) {
    this.paginationChanged.emit({
      pageSize: this.pageSize,
      pageNumber: backToPageOne ? 1 : this.pageNumber()
    })
  }
}
