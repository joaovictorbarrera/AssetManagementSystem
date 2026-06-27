import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PageLoadingService } from './core/services/page-loading.service';
import { SpinningWheel } from './core/components/spinning-wheel/spinning-wheel';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SpinningWheel],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  constructor(readonly pageLoading: PageLoadingService) {}
}
