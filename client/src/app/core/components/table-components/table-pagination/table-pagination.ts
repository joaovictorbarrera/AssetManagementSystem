import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, signal } from '@angular/core';
import Pagination from '../../../DTOs/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table-pagination',
  imports: [FormsModule],
  templateUrl: './table-pagination.html',
  styleUrl: './table-pagination.scss',
})
export class TablePagination {
  @Input() colspan!: number
  @Input() pagination!: Pagination
  @Output() paginationChanged = new EventEmitter<{ pageSize: number; pageNumber: number }>();

  pageNumber = signal(this.pagination?.pageNumber ?? 1)
  pageSize = signal(this.pagination?.pageSize ?? 25)

  get showingFrom() {
    return this.pagination.pageSize * (this.pageNumber() - 1) + 1;
  }

  get showingTo() {
    return Math.min(
      this.pagination.pageSize * this.pageNumber(),
      this.pagination.totalCount
    );
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
      pageSize: this.pageSize(),
      pageNumber: backToPageOne ? 1 : this.pageNumber()
    })
  }
}
