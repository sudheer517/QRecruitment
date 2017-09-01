import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyResponseDto } from '../../../RemoteServicesProxy';
import { SurveyService } from '../../services/survey.service'

@Component({
  selector: 'appc-surveychallenge',
  templateUrl: './surveychallenge.component.html',
  styleUrls: ['./surveychallenge.component.scss']
})
export class SurveychallengeComponent implements OnInit {
    myform: FormGroup;
    private surveyResponses: SurveyResponseDto[];
    constructor(private surveyService: SurveyService, private router: Router, private activatedRoute: ActivatedRoute, fb: FormBuilder) {
        this.myform = fb.group({
            'name' : ['Banu']
        });
    }

    ngOnInit() {
        this.surveyService.GetSurveyChallenge().subscribe(
            challenge => {
                this.surveyResponses = challenge;
            },
            error => { console.log(error); }
        );    
    }

    public SubmitResponses()
    {
        this.surveyService.SaveSurveyResponse(this.surveyResponses).subscribe(
            res => {
                this.router.navigate(["../challenges"], { relativeTo: this.activatedRoute });
            }

        );
    }

}
