import { Routes, RouterModule }  from '@angular/router';
import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';
import { TestFinishedComponent } from './components/testFinished/testFinished.component';
import { CandidateDetailsComponent } from './components/candidateDetails/candidateDetails.component';
import { CandidateDetailsGuard } from './services/candidateDetailsGuard.services';
const routes: Routes = [
  {
    path: '', component: CandidateComponent , children : [
      { path: '', component: CandidateDetailsComponent },
      { path: 'instructions', component: InstructionsComponent, canActivate: [CandidateDetailsGuard] },
      { path: 'challenge', component: ChallengeComponent, canActivate: [CandidateDetailsGuard] },
      { path: 'testFinished', component: TestFinishedComponent },
      { path: 'candidateDetails', component: CandidateDetailsComponent }
    ]
  }
];

export const routing = RouterModule.forChild(routes);
