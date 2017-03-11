import { NgModule } from '@angular/core';

import { routing } from './candidate.routes';
import { SharedModule } from '../shared/shared.module';
import { ChartModule } from 'angular2-highcharts';

import { ReactiveFormsModule } from '@angular/forms';
import { ModalModule, ButtonsModule } from 'ng2-bootstrap';
import { SelectModule } from 'ng2-select';

import { CandidateComponent } from './candidate.component';

@NgModule({
    imports: [
        routing, 
        SharedModule, 
        ChartModule.forRoot(require('highcharts')),
        ReactiveFormsModule,
        ModalModule.forRoot(),
        ButtonsModule.forRoot(),
        SelectModule],
    declarations: [
       CandidateComponent
    ],
    providers: [  ]
})
export class CandidateModule { }
