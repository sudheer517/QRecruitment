import { Component, OnInit, OnDestroy, trigger, state, style, transition, animate, keyframes } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestService } from '../../../services/test.service';
import { TestDto, OptionDto, ChallengeDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: '[appc-test-detail]',
    templateUrl: './testDetail.component.html',
    styleUrls: ['./testDetail.component.scss'],
    animations: [
      trigger("visibility",[
          state("shown", style({
            left: '0px'
          })),
          state("hidden", style({
            left: '-350px',
          })),
          transition("* <=> *", animate("200ms"))
      ])
  ],
})
export class TestDetailComponent implements OnInit, OnDestroy {

    visibility: string = "hidden";
    setDisplayNoneForSideNav = true;
    isNavbarCollapsed: boolean;

    testId: number;
    test: TestDto;
    
    private routeParamSubscription: any
    constructor(private testService: TestService, private route: ActivatedRoute){

    }

    ngOnInit(){
        this.routeParamSubscription = this.route.params.subscribe(params => {
            this.testId = +params['testId']; // (+) converts string 'id' to a number
            this.testService.GetFinishedTestDetail(this.testId).subscribe(
                test => {
                    this.test = test;
                },
                error => {
                    console.log(error);
                }
            )
        });
    }

    toggleNavButton(){
        this.setDisplayNoneForSideNav = false;
        this.isNavbarCollapsed = !this.isNavbarCollapsed;
        this.visibility = (this.visibility === "hidden" ? "shown" : "hidden");
    }

    keys(): Array<string> {
        return Object.keys(this.test.LabelDiffAnswers);
    }

    isCorrect(challenge: ChallengeDto){
        let correctOptions = challenge.Question.Options.filter(o => o.IsAnswer === true);

        let intersection = challenge.CandidateSelectedOptions.map(cso => cso.OptionId).filter(n => {
            return correctOptions.map(o => o.Id).indexOf(n) !== -1;
        });

        //console.log(intersection);
        if (intersection.length === correctOptions.length && challenge.CandidateSelectedOptions.length === correctOptions.length) {
            //console.log("right answer for id:" + challenge.Id);
            return true;
        }

        return false;
    }



    hasCandidateSelected(option: OptionDto, currentChallenge: ChallengeDto): boolean {
        var test = this.test;
        if (currentChallenge.CandidateSelectedOptions.filter(cso => cso.OptionId === option.Id).length == 1)
            return true;

        return false;
    }

    ngOnDestroy() {
        this.routeParamSubscription.unsubscribe();
    }
}