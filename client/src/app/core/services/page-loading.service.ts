import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PageLoadingService {
  readonly isLoading = signal(false);

  setLoading(loading: boolean) {
    this.isLoading.set(loading);
  }
}
