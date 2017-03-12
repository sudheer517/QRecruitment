import { Routes, RouterModule }  from '@angular/router';
import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';
import { TestFinishedComponent } from './components/testFinished/testFinished.component';

const routes: Routes = [
  {
    path: '', component: CandidateComponent , children : [
      { path: '', component: InstructionsComponent },
      { path: 'challenge', component: ChallengeComponent },
      { path: 'testFinished', component: TestFinishedComponent }
    ]
  }
];

export const routing = RouterModule.forChild(routes);
