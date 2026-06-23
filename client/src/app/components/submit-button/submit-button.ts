import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-submit-button',
  templateUrl: './submit-button.html',
})
export class SubmitButton {
  @Input() loading: boolean = false;
  @Input() disabled: boolean = false;
}
