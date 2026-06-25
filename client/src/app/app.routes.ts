import { Routes } from '@angular/router';
import { MainLayout } from './core/layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';
import { Dashboard } from './pages/dashboard/dashboard';
import { Requests } from './pages/requests/requests';
import { Inventory } from './pages/inventory/inventory';
import { Users } from './pages/users/users';
import { Review } from './pages/review/review';
import { Unauthorized } from './pages/unauthorized/unauthorized';
import { Login } from './pages/login/login';
import { roleGuard } from './core/guards/role.guard';
import { Role } from './core/enums/role';

export const routes: Routes = [
  {
    path: '',
    canActivateChild: [authGuard],
    component: MainLayout,
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: Dashboard
      },
      {
        path: 'requests',
        component: Requests
      },
      {
        path: 'inventory',
        component: Inventory,
        canActivate: [roleGuard],
        data: {
          roles: [Role.Admin, Role.AssetManager]
        }
      },
      {
        path: 'users',
        component: Users,
        canActivate: [roleGuard],
        data: {
          roles: [Role.Admin]
        }
      },
      {
        path: 'review',
        component: Review,
        canActivate: [roleGuard],
        data: {
          roles: [Role.Admin, Role.AssetManager]
        }
      },
      {
        path: 'unauthorized',
        component: Unauthorized
      }
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
