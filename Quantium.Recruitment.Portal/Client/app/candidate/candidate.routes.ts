import { Routes, RouterModule }  from '@angular/router';
import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';
import { TestFinishedComponent } from './components/testFinished/testFinished.component';
import { CandidateDetailsComponent } from './components/candidateDetails/candidateDetails.component';
import { InstructionsGuard } from './services/instructions.guard';
import { TestGuard } from './services/test.guard';
import { DetailsGuard } from './services/candidateDetails.guard';
import { FeedbackComponent } from './components/feedback/feedback.component';
import { UnassignedTestComponent } from './components/unassignedTest/unassignedTest.component';
import { SurveyGuard } from './services/survey.guard';
import { SurveychallengeComponent } from './components/surveychallenge/surveychallenge.component';
import { ChallengesComponent } from './components/challenges/challenges.component';


const routes: Routes = [
  {
    path: '', component: CandidateComponent , children : [
      { path: '', component: CandidateDetailsComponent, canActivate: [DetailsGuard] },
      { path: 'candidateDetails', component: CandidateDetailsComponent, canActivate: [DetailsGuard] },
      { path: 'challenges', component: ChallengesComponent },
      { path: 'instructions', component: InstructionsComponent, canActivate: [TestGuard, InstructionsGuard]},
      { path: 'challenge', component: ChallengeComponent, canActivate: [TestGuard, InstructionsGuard] },
      { path: 'testFinished', component: TestFinishedComponent},
      { path: 'feedback', component: FeedbackComponent },
      { path: 'unassignedTest', component: UnassignedTestComponent },
      { path: 'survey', component: SurveychallengeComponent, canActivate: [SurveyGuard]},
    
    ]
  }
];

export const routing = RouterModule.forChild(routes);
