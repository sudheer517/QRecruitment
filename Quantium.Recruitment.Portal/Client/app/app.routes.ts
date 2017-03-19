import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { QAuthGuard } from './qauth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  // Lazy async modules
  {
    path: 'candidate', loadChildren: './candidate/candidate.module#CandidateModule'
  },
  {
    path: 'admin', loadChildren: './admin/admin.module#AdminModule', canLoad: [QAuthGuard]
  }
];

export const routing = RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules });
