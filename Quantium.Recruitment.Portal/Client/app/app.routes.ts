import { Routes, RouterModule, PreloadAllModules } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'admin', pathMatch: 'full' },
  // Lazy async modules
  {
    path: 'login', loadChildren: './+login/login.module#LoginModule'
  },
  {
    path: 'register', loadChildren: './+register/register.module#RegisterModule'
  },
  {
    path: 'profile', loadChildren: './+profile/profile.module#ProfileModule'
  },
  {
    path: 'home', loadChildren: './home/home.module#HomeModule'
  },
  {
    path: 'examples', loadChildren: './+examples/examples.module#ExamplesModule'
  }
];

export const routing = RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules });
