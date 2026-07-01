import { Component, ElementRef, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, signal, SimpleChanges } from '@angular/core';
import LabelValuePair from '../../DTOs/shared/label-value-pair';

@Component({
  selector: 'app-dropdown',
  imports: [],
  templateUrl: './dropdown.html',
  styleUrl: './dropdown.scss',
})
export class Dropdown implements OnInit {
  private static openDropdown?: Dropdown;

  @Input() title!: string;
  @Input() list!: LabelValuePair[];
  @Input() initialSelection?: string;
  @Input() enableAll = false;

  @Output() dropdownChanged = new EventEmitter<string>();

  open = false;

  currentValue = '';
  currentLabel = '';

  private lastValue = '';

  constructor(private element: ElementRef<HTMLElement>) {}

  ngOnInit(): void {
    if (this.initialSelection) {
      this.currentValue = this.initialSelection
      this.currentLabel = this.list.find(x => x.value === this.initialSelection)?.label ?? ''
    }

    if (this.enableAll) {
      this.currentValue = 'all'
      this.currentLabel = 'All'
    }
  }

  toggle() {
    if (this.open) {
      this.close();
      return;
    }

    Dropdown.openDropdown?.close();

    Dropdown.openDropdown = this;
    this.open = true;
  }

  select(value: string) {
    if (value === this.currentValue) {
      this.close()
      return
    }

    this.lastValue = this.currentValue
    this.currentValue = value;
    this.currentLabel = value === 'all' ? 'All' : this.list.find(x => x.value === value)?.label ?? ''
    this.open = false;

    this.dropdownChanged.emit(value === 'all' ? '' : value);
  }

  revert() {
    this.currentValue = this.lastValue
    this.currentLabel = this.list.find(x => x.value === this.currentValue)?.label ?? ''
  }

  @HostListener('document:click')
  close() {
    this.open = false;

    if (Dropdown.openDropdown === this) {
      Dropdown.openDropdown = undefined;
    }
  }
}
