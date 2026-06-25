import { Component, EventEmitter, Output } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { NgIcon } from '@ng-icons/core'

@Component({
  selector: 'app-search-bar',
  imports: [NgIcon, FormsModule],
  templateUrl: './search-bar.html',
  styleUrl: './search-bar.scss',
})
export class SearchBar {
  searchText = ''

  @Output() search = new EventEmitter<string>()

  onSearch() {
    this.search.emit(this.searchText.trim())
  }

  onEnter(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onSearch()
    }
  }
}
