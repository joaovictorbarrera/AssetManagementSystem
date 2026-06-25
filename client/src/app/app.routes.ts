import { Routes } from '@angular/router';
import { MainLayout } from './core/layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';
import { Dashboard } from './pages/dashboard/dashboard';
import { Requests } from './pages/requests/requests';
import { Inventory } from './pages/inventory/inventory';
import { Review } from './pages/review/review';
import { Users } from './pages/users/users';
import { Login } from './pages/login/login';

export const routes: Routes = [
  {
    path: '',
    canActivateChild: [authGuard],
    component: MainLayout,
    children: [
      { path: '', redirectTo: "dashboard", pathMatch: 'full' },
      { path: 'dashboard', component: Dashboard },
      { path: 'requests', component: Requests },
      { path: 'inventory', component: Inventory },
      { path: 'review', component: Review },
      { path: 'users', component: Users }
    ]
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: '**',
    redirectTo: ''
  }
];
