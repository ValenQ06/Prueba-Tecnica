import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { HomeComponent } from './pages/home/home';

export const routes: Routes = [
  { path: '', component: LoginComponent },

  { path: 'dashboard', component: DashboardComponent },

  { path: 'home', component: HomeComponent },

  {
    path: 'comerciante',
    loadComponent: () =>
      import('./pages/comerciante-form/comerciante-form').then((m) => m.ComercianteFormComponent),
  },
  {
    path: 'comerciante/:id',
    loadComponent: () =>
      import('./pages/comerciante-form/comerciante-form').then((m) => m.ComercianteFormComponent),
  },
];
