import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({ providedIn: 'root' })
export class UserEventsService {
  private _usersChanged = new Subject<void>();
  usersChanged$ = this._usersChanged.asObservable();

  emitUsersChanged(): void {
    this._usersChanged.next();
  }
}
