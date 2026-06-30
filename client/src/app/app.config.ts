import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth-interceptor';
import { provideIcons } from '@ng-icons/core';
import { heroCalendar, heroCheck, heroChevronLeft, heroChevronRight, heroClipboardDocumentCheck, heroComputerDesktop, heroInboxArrowDown, heroListBullet, heroMagnifyingGlass, heroPlus, heroServerStack, heroShieldCheck, heroUser, heroXMark } from '@ng-icons/heroicons/outline';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([
      authInterceptor
    ])),
    provideIcons({
      heroMagnifyingGlass,
      heroComputerDesktop,
      heroInboxArrowDown,
      heroListBullet,
      heroPlus,
      heroServerStack,
      heroShieldCheck,
      heroUser,
      heroChevronLeft,
      heroChevronRight,
      heroClipboardDocumentCheck,
      heroXMark,
      heroCheck,
      heroCalendar
    })
  ]
}
