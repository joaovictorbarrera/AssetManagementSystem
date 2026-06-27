import { TitleCasePipe } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dropdown',
  imports: [FormsModule, TitleCasePipe],
  templateUrl: './dropdown.html',
  styleUrl: './dropdown.scss',
})
export class Dropdown implements OnChanges {
  @Input() name!: string
  @Input() list!: string[]
  @Input() defaultSelected?: string
  @Input() enableAll = false
  @Output() dropdownChanged = new EventEmitter<string>()

  value = ''

  ngOnChanges(changes: SimpleChanges) {
    if (changes['defaultSelected'] && this.defaultSelected !== undefined) {
      this.value = this.defaultSelected
      return
    }

    if (changes['enableAll'] && this.enableAll) {
      this.value = 'all'
      return
    }
  }

  onChange() {
    this.dropdownChanged.emit(this.value)
  }
}
