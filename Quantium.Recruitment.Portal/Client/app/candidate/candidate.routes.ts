import { Routes, RouterModule }  from '@angular/router';
import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';

const routes: Routes = [
  {
    path: '', component: CandidateComponent , children : [
      { path: '', component: InstructionsComponent },
      { path: 'test', component: ChallengeComponent }
    ]
  }
];

export const routing = RouterModule.forChild(routes);
