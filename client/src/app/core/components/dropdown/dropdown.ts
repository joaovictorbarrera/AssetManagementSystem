import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dropdown',
  imports: [FormsModule],
  templateUrl: './dropdown.html',
  styleUrl: './dropdown.scss',
})
export class Dropdown {
  @Input() name!: string
  @Input() list!: string[]
  @Output() dropdownChanged = new EventEmitter<string>()

  value = "all"

  onChange() {
    this.dropdownChanged.emit(this.value)
  }
}
