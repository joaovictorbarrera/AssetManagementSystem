import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AssetEventsService {
  private _assetsChanged = new Subject<void>();
  assetsChanged$ = this._assetsChanged.asObservable();

  emitAssetsChanged(): void {
    this._assetsChanged.next();
  }
}
