import { Injectable, Injector, Type } from '@angular/core';
import {
  Overlay, OverlayRef,
  GlobalPositionStrategy
} from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';

@Injectable({ providedIn: 'root' })
export class DrawerService {
  private overlayRef: OverlayRef | null = null;

  constructor(private overlay: Overlay, private injector: Injector) {}

  open<T extends object>(component: Type<T>, inputs?: Partial<T>): void {
    this.close(); // prevent stacking

    this.overlayRef = this.overlay.create({
      hasBackdrop: true,
      backdropClass: 'drawer-backdrop',
      positionStrategy: this.overlay
        .position()
        .global()
        .right('0')
        .top('0')
        .bottom('0'),
      width: '480px',
    });

    const portal = new ComponentPortal(component, null, this.injector);
    const ref = this.overlayRef.attach(portal);

    // Apply inputs
    if (inputs) {
      Object.assign(ref.instance, inputs);
      ref.changeDetectorRef.markForCheck();
    }

    // Close on backdrop click
    this.overlayRef.backdropClick().subscribe(() => this.close());
  }

  close(): void {
    this.overlayRef?.dispose();
    this.overlayRef = null;
  }
}
