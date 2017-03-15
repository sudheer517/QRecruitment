import { Component, OnInit } from '@angular/core';
import { SurveyService } from '../../../services/survey.service';
import { SurveyDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-view-surveys',
    templateUrl: './viewSurveys.component.html'
})
export class ViewSurveysComponent implements OnInit{
    surveys: SurveyDto[];
   constructor(private surveyService: SurveyService){
   }

   ngOnInit(){
     this.surveyService.GetAll().subscribe(
       surveys => {
         this.surveys = surveys;
         console.log(surveys);
       },
       error => console.log(error)
     )
   }
}