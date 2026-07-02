import { afterNextRender, Component, ElementRef, EventEmitter, HostListener, inject, Injector, Input, OnChanges, Output, signal } from '@angular/core'
import LabelValuePair from '../../DTOs/shared/label-value-pair'

@Component({
  selector: 'app-dropdown',
  imports: [],
  templateUrl: './dropdown.html',
  styleUrl: './dropdown.scss',
})
export class Dropdown implements OnChanges {
  private static openDropdown?: Dropdown

  @Input() title!: string
  @Input() list!: LabelValuePair[]
  @Input() initialSelection?: string
  @Input() enableAll = false

  @Output() dropdownChanged = new EventEmitter<string>()

  open = false
  openUpward = signal(false)

  currentValue = ''
  currentLabel = ''

  private lastValue = ''

  constructor(
    private elementRef: ElementRef<HTMLElement>,
    private injector: Injector
  ) {}

  ngOnChanges(): void {
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
      this.close()
      return
    }

    Dropdown.openDropdown?.close()

    Dropdown.openDropdown = this
    this.open = true

    // Wait for next render so browser can paint element first
    // before calculating its height
    afterNextRender(() => this.updateDirection(), { injector: this.injector })
  }

  select(value: string) {
    if (value === this.currentValue) {
      this.close()
      return
    }

    this.lastValue = this.currentValue
    this.currentValue = value
    this.currentLabel = value === 'all' ? 'All' : this.list.find(x => x.value === value)?.label ?? ''
    this.open = false

    this.dropdownChanged.emit(value === 'all' ? '' : value)
  }

  revert() {
    this.currentValue = this.lastValue
    this.currentLabel = this.list.find(x => x.value === this.currentValue)?.label ?? ''
  }

  @HostListener('document:click')
  close() {
    this.open = false

    if (Dropdown.openDropdown === this) {
      Dropdown.openDropdown = undefined
    }
  }

  updateDirection() {
    const buttonEl = this.elementRef.nativeElement.querySelector('.dropdown') as HTMLElement | null
    const menuEl = this.elementRef.nativeElement.querySelector('.dropdown-menu') as HTMLElement | null

    if (!buttonEl || !menuEl) {
      return
    }

    const buttonRect = buttonEl.getBoundingClientRect()
    const menuHeight = menuEl.offsetHeight

    // difference between total window height and bottom of menu button
    // is the amount of space left below. if space below is less than menu height, flip it
    const spaceBelow = window.innerHeight - buttonRect.bottom

    const offset = 50
    this.openUpward.set(spaceBelow - offset < menuHeight)
  }
}
