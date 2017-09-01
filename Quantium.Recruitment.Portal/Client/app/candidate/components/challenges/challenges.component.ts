import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ChallengeService } from '../../services/challenge.service'
import { SurveyService } from '../../services/survey.service'

@Component({
  selector: 'appc-challenges',
  templateUrl: './challenges.component.html',
  styleUrls: ['./challenges.component.scss']
})
export class ChallengesComponent implements OnInit {

    public testAssigned: boolean ;
    public testFinished: boolean ;
    public surveyAssigned: boolean ;
    public surveyFinished: boolean;
  
    
   constructor(private router: Router, private activatedRoute: ActivatedRoute, private challengeService: ChallengeService, private surveyService: SurveyService) {  }

   ngOnInit() {
       let testAssigned = this.challengeService.IsTestAssigned().map(
            isAssigned => {
                this.testAssigned = isAssigned;              
                return isAssigned;
            },
            error => { console.log(error); },         
       );

       let testFinished =
           this.challengeService.IsTestFinished().map(
                        isFinished => {
                            this.testFinished = isFinished;
                            return isFinished;
                        },
                        error => { console.log(error); }                        
                    );

       let surveyAssigned = this.surveyService.IsSurveyAssigned().map(
            isAssigned => {
                this.surveyAssigned = isAssigned;             
                return isAssigned;
            },
            error => { console.log(error); }
        );

       let surveyFinished = this.surveyService.IsSurveyFinished().map(
                        isFinished => {
                            this.surveyFinished = isFinished;
                            return isFinished;
                        },
                        error => { console.log(error); }
                    );



         Observable.forkJoin([testAssigned, testFinished, surveyAssigned, surveyFinished]).subscribe(
             a => {
                 if (this.testAssigned && this.testFinished && !this.surveyAssigned)
                 {
                     this.router.navigate(["../testFinished"], { relativeTo: this.activatedRoute });
                 }
                 if (this.surveyAssigned && this.surveyFinished && !this.testAssigned)
                 {
                     this.router.navigate(["../testFinished"], { relativeTo: this.activatedRoute });
                 }
                 if (this.testAssigned && this.surveyAssigned && this.testFinished && this.surveyFinished)
                 {
                   this.router.navigate(["../testFinished"], { relativeTo: this.activatedRoute });
                 }
                 if (!this.testAssigned && !this.surveyAssigned)
                 {
                     this.router.navigate(["../unassignedTest"], { relativeTo: this.activatedRoute });
                 }
           },
           error => { console.log(error); }
           )

  }

  takeTest() {
      this.router.navigate(["../instructions"], { relativeTo: this.activatedRoute });
  }

  takeSurvey() {
      this.router.navigate(["../survey"], { relativeTo: this.activatedRoute });
  }
}
