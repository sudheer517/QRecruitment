import { Routes, RouterModule }  from '@angular/router';
import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';
import { TestFinishedComponent } from './components/testFinished/testFinished.component';
import { CandidateDetailsComponent } from './components/candidateDetails/candidateDetails.component';
import { InstructionsGuard } from './services/instructions.guard';
import { TestGuard } from './services/test.guard';
import { DetailsGuard } from './services/candidateDetails.guard';

// const routes: Routes = [
//   {
//     path: '', component: CandidateComponent , children : [
//       { path: '', component: InstructionsComponent, canActivate: [InstructionsGuard, TestGuard] },
//       { path: 'instructions', component: InstructionsComponent, canActivate: [InstructionsGuard] },
//       { path: 'challenge', component: ChallengeComponent },
//       { path: 'testFinished', component: TestFinishedComponent },
//       { path: 'candidateDetails', component: CandidateDetailsComponent}
//     ]
//   }
// ];

const routes: Routes = [
  {
    path: '', component: CandidateComponent , children : [
      { path: '', component: CandidateDetailsComponent, canActivate: [DetailsGuard] },
      { path: 'candidateDetails', component: CandidateDetailsComponent, canActivate: [DetailsGuard]},
      { path: 'instructions', component: InstructionsComponent, canActivate: [InstructionsGuard]},
      { path: 'challenge', component: ChallengeComponent, canActivate: [InstructionsGuard] },
      { path: 'testFinished', component: TestFinishedComponent },
    ]
  }
];

export const routing = RouterModule.forChild(routes);
