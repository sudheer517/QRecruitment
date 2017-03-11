import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { QAuthGuard } from './qauth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  // Lazy async modules
  {
    path: 'candidate', loadChildren: './candidate/candidate.module#CandidateModule'
  },
  {
    path: 'register', loadChildren: './+register/register.module#RegisterModule'
  },
  {
    path: 'profile', loadChildren: './+profile/profile.module#ProfileModule'
  },
  {
    path: 'admin', loadChildren: './admin/admin.module#AdminModule', canLoad: [QAuthGuard]
  },
  {
    path: 'examples', loadChildren: './+examples/examples.module#ExamplesModule'
  }
];

export const routing = RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules });
