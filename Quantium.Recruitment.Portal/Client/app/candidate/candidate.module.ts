import { NgModule } from '@angular/core';

import { routing } from './candidate.routes';
import { SharedModule } from '../shared/shared.module';

import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';
import { ChallengeService } from './services/challenge.service';
import { CandidateService } from './services/candidate.service';
import { FeedbackService } from './services/feedback.service';
import { InstructionsGuard } from './services/instructions.guard';
import { TestGuard } from './services/test.guard';
import { DetailsGuard } from './services/candidateDetails.guard';

import { TestFinishedComponent } from './components/testFinished/testFinished.component';
import { CandidateDetailsComponent } from './components/candidateDetails/candidateDetails.component';
import { FeedbackComponent } from './components/feedback/feedback.component';
import { UnassignedTestComponent } from './components/unassignedTest/unassignedTest.component';

@NgModule({
    imports: [
        routing, 
        SharedModule
        ],
    declarations: [
       CandidateComponent,
       InstructionsComponent,
       ChallengeComponent,
       TestFinishedComponent,
       CandidateDetailsComponent,
       FeedbackComponent,
       UnassignedTestComponent
    ],
    providers: [ ChallengeService, CandidateService, FeedbackService, InstructionsGuard, DetailsGuard, TestGuard ]
})
export class CandidateModule { }
