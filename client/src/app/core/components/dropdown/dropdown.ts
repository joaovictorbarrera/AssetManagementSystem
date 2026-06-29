import { TitleCasePipe } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dropdown',
  imports: [FormsModule, TitleCasePipe],
  templateUrl: './dropdown.html',
  styleUrl: './dropdown.scss',
})
export class Dropdown implements OnChanges {
  @Input() label!: string
  @Input() list!: string[]
  @Input() initialSelection?: string
  @Input() enableAll = false
  @Output() dropdownChanged = new EventEmitter<string>()

  currentValue = ''
  private initialValue = ''

  ngOnChanges(changes: SimpleChanges) {
    if (changes['initialSelection'] && this.initialSelection !== undefined) {
      this.currentValue = this.initialSelection
      this.initialValue = this.initialSelection
    } else if (changes['enableAll'] && this.enableAll) {
      this.currentValue = 'all'
      this.initialValue = 'all'
    }
  }

  onChange(newValue: string) {
    this.dropdownChanged.emit(newValue)
  }

  revert() {
    // Needs to update on the next cycle
    setTimeout(() => {
      this.currentValue = this.initialValue
    })
  }
}
