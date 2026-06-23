import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      { path: '', redirectTo: "dashboard", pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'requests', loadComponent: () => import('./pages/requests/requests').then(m => m.Requests) },
      { path: 'inventory', loadComponent: () => import('./pages/inventory/inventory').then(m => m.Inventory) },
      { path: 'review', loadComponent: () => import('./pages/review/review').then(m => m.Review) },
      { path: 'users', loadComponent: () => import('./pages/users/users').then(m => m.Users) },
    ]
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login').then(m => m.Login)
  }
];
