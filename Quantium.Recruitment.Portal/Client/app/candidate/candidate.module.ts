import { NgModule } from '@angular/core';

import { routing } from './candidate.routes';
import { SharedModule } from '../shared/shared.module';

import { CandidateComponent } from './candidate.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { ChallengeComponent } from './components/challenge/challenge.component';

@NgModule({
    imports: [
        routing, 
        SharedModule
        ],
    declarations: [
       CandidateComponent,
       InstructionsComponent,
       ChallengeComponent
    ],
    providers: [  ]
})
export class CandidateModule { }
