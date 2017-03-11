import { Routes, RouterModule }  from '@angular/router';
import { CandidateComponent } from './candidate.component';


const routes: Routes = [
  {
    path: '', component: CandidateComponent
  }
];

export const routing = RouterModule.forChild(routes);
