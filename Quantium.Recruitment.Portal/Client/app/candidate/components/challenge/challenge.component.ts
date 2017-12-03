import { Component, OnInit, ViewChild, trigger, state, style, transition, animate, keyframes  } from '@angular/core';
import { ChallengeService } from '../../services/challenge.service';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionDto, ChallengeDto, CandidateSelectedOptionDto } from '../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Observable } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: '[appc-challenge]',
  styleUrls: ['./challenge.component.scss'],
  templateUrl: './challenge.component.html',
  animations: [
      trigger("visibility",[
          state("shown", style({
            left: '0px'
          })),
          state("hidden", style({
            left: '-270px',
          })),
          transition("* <=> *", animate("200ms"))
      ])
  ],
})
export class ChallengeComponent implements OnInit{
    @ViewChild('confirmation') confirmationModal: ModalDirective;
    @ViewChild('progress') progressModal: ModalDirective;
    @ViewChild('timeUp') timeUpModal: ModalDirective;

    visibility: string = "hidden";
    setDisplayNoneForSideNav = true;
    isNavbarCollapsed: boolean;

    constructor(private challengeService: ChallengeService, private router: Router, private activatedRoute: ActivatedRoute) {
    }

    totalTestTime: string;
    remainingTestTime: string;

    challengesAnswered: Boolean[];
    currentChallengeNumber: number;
    remainingChallenges: number;

    question: QuestionDto;
    selectedCheckboxOptions: boolean[]; 
    selectedRadioOption: number;

    private currentChallenge: ChallengeDto;
    private endDateTime: string;
    private startDateTime: string;
    
    ticks = 0;
    private timer;
    private sub: Subscription;

    isTimeUp = false;
    
    ngOnInit(){
        this.getNextChallenge();
        this.timer = Observable.timer(1000,1000);
    }

    reduceTimerBy1Second(){
        if (this.ticks <= 0) {
            this.sub.unsubscribe();
            this.isTimeUp = true;
            this.timeUpModal.show();
            this.endDateTime = new Date().toUTCString();
            this.postChallenge(false);
        }
        else {
            this.ticks = this.ticks - 1;
        }

    }

    private getNextChallenge(): void {
            this.challengeService.GetNextChallenge().subscribe(
                challenge => {
                    let isFinished: any = challenge;

                    if (isFinished === "Finished") {
                        this.gotoFeedback();
                    }
                    else {
                        if (this.timeUpModal.isShown) {
                            this.timeUpModal.hide();
                        }
                        
                        this.selectedCheckboxOptions = new Array<boolean>(challenge.Question.Options.length);
                        this.selectedRadioOption = null;

                        this.currentChallenge = challenge;
                        this.question = challenge.Question;
                        this.currentChallengeNumber = challenge.currentChallenge;
                        this.remainingChallenges = challenge.RemainingChallenges;
                        this.challengesAnswered = challenge.ChallengesAnswered;

                        this.startDateTime = new Date().toUTCString();
                        this.totalTestTime = challenge.TotalTestTimeInMinutes;
                        this.remainingTestTime = challenge.RemainingTestTimeInMinutes;

                        this.isTimeUp = false;
                        this.ticks = challenge.Question.TimeInSeconds;
                        this.sub = this.timer.subscribe(t => this.reduceTimerBy1Second());
                    }

                    this.progressModal.hide();
                }, 
                error => {
                    console.log("new question retrieval failed");
                });
    }

    private fillAndPostChallenge() {
        this.progressModal.show();
        this.sub.unsubscribe();

        this.endDateTime = new Date().toUTCString();
        this.postChallenge();
    }

    private nextQuestion(): void {
        this.timeUpModal.hide();
        this.sub.unsubscribe();
        this.getNextChallenge();
    }

    showFinishTestConfirm() {
        this.confirmationModal.show(); 
    }

    private postChallenge(shouldGetNextQuestion: boolean = true): void {
        this.sub.unsubscribe();
        this.currentChallenge.StartTime = this.startDateTime;
        this.currentChallenge.AnsweredTime = this.endDateTime;
        let challengeDto = this.currentChallenge;
        let candidateSelectedOptionDtoItems: CandidateSelectedOptionDto[] = [];

        challengeDto.QuestionId = challengeDto.Question.Id;

        let isRadio = this.question.IsRadio;
        if(isRadio && this.selectedRadioOption){
            let candidateSelectedOptionDto = new CandidateSelectedOptionDto();
            candidateSelectedOptionDto.ChallengeId = this.currentChallenge.Id;
            candidateSelectedOptionDto.OptionId = this.selectedRadioOption;
            candidateSelectedOptionDtoItems.push(candidateSelectedOptionDto);
        }
        else{
            let selectedOptionIds = this.selectedCheckboxOptions;
            selectedOptionIds.forEach((item, index) => {
                if (item === true) {
                    let candidateSelectedOptionDto = new CandidateSelectedOptionDto();
                    candidateSelectedOptionDto.ChallengeId = this.currentChallenge.Id;
                    candidateSelectedOptionDto.OptionId = this.question.Options[index].Id;
                    candidateSelectedOptionDtoItems.push(candidateSelectedOptionDto);
                }
            });
        }

        challengeDto.CandidateSelectedOptions = candidateSelectedOptionDtoItems;
        
        this.challengeService.PostChallenge(challengeDto).subscribe(
            result => {
                if (shouldGetNextQuestion) {
                    this.getNextChallenge();
                }
            },
            reason => {
                console.log("answer posting failed");
            });
    }

    private finishAllChallenges(): void {
        this.endDateTime = new Date().toUTCString();
        this.postChallenge(false);
        this.sub.unsubscribe();
        this.challengeService.FinishAllChallenges(this.currentChallenge.TestId).subscribe(
            response => {
                this.gotoFeedback();
            },
            error => {
                console.log(error);
            });;
    }

    public gotoFeedback() {
        this.router.navigate(["../feedback"], { relativeTo: this.activatedRoute });
    }

    public toggleNavButton() {
        this.setDisplayNoneForSideNav = false;
        this.isNavbarCollapsed = !this.isNavbarCollapsed;
        this.visibility = (this.visibility === "hidden" ? "shown" : "hidden");
    }
}
