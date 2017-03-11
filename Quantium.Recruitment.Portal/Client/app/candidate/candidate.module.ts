import { NgModule } from '@angular/core';

import { routing } from './candidate.routes';
import { SharedModule } from '../shared/shared.module';

import { CandidateComponent } from './candidate.component';

@NgModule({
    imports: [
        routing, 
        SharedModule
        ],
    declarations: [
       CandidateComponent
    ],
    providers: [  ]
})
export class CandidateModule { }
