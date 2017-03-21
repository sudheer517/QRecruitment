import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestService } from '../../../services/test.service';
import { TestDto, OptionDto, ChallengeDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-test-detail',
    templateUrl: './testDetail.component.html'
})
export class TestDetailComponent implements OnInit, OnDestroy {

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