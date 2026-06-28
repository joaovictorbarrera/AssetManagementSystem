import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AssetEventsService {
  private _assetUpdated = new Subject<void>();
  assetUpdated$ = this._assetUpdated.asObservable();

  emitAssetUpdated(): void {
    this._assetUpdated.next();
  }
}
