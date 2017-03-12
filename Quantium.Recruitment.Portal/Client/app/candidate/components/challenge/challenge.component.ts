import { Component, OnInit, ViewChild } from '@angular/core';
import { ChallengeService } from '../../services/challenge.service';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionDto, ChallengeDto, CandidateSelectedOptionDto } from '../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';


@Component({
  selector: '[appc-challenge]',
  styleUrls: ['./challenge.component.scss'],
  templateUrl: './challenge.component.html',
})
export class ChallengeComponent implements OnInit{
  @ViewChild('confirmation') confirmationModal: ModalDirective;
  @ViewChild('progress') progressModal: ModalDirective;

  constructor(private challengeService: ChallengeService, private router: Router, private activatedRoute: ActivatedRoute) {
  }

  totalTestTime: string;
  remainingTestTime: string;

  challengesAnswered: Boolean[];
  currentChallengeNumber: number;

  question: QuestionDto;
  remainingChallenges : number;

  selectedCheckboxOptions: boolean[]; 
  selectedRadioOption: number;

  private currentChallenge: ChallengeDto;
  private endDateTime: string;
  private startDateTime: string;
  private currentTestId: number;

  ngOnInit(){
    this.getNextChallenge();
  }

  //private currentTestId: number;

  private getNextChallenge(): void {
        this.challengeService.GetNextChallenge().subscribe(
            challenge => {
                let isFinished: any = challenge;
                if(isFinished === "Finished"){
                     this.router.navigate(["../testFinished"], { relativeTo: this.activatedRoute});
                }
                else{
                    this.selectedCheckboxOptions = new Array<boolean>(challenge.Question.Options.length);
                    this.currentTestId = challenge.TestId;
                    // this.$timeout.cancel(this.myTimer);
                    // this.myTimer = this.$timeout(() => { this.showConfirm() }, (result.data.Question.TimeInSeconds * 1000));
                    // this.startDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
                    this.startDateTime = new Date().toUTCString();
                    this.currentChallenge = challenge;
                    
                    this.question = challenge.Question;
                    this.currentChallengeNumber = challenge.currentChallenge;
                    this.remainingChallenges = challenge.RemainingChallenges;
                    this.challengesAnswered = challenge.ChallengesAnswered;
                    this.totalTestTime = challenge.TotalTestTimeInMinutes;
                    this.remainingTestTime = challenge.RemainingTestTimeInMinutes;
                    // this.$scope.isRadioQuestion = result.data.Question.IsRadio;
                    // this.$log.info("new question retrieved");
                    // this.setTimer(result.data.Question.TimeInSeconds);
                }
                this.progressModal.hide();
            }, 
            error => {
                console.log("new question retrieval failed");
            });
  }

  private fillAndPostChallenge() {
    //this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
    this.progressModal.show();

    console.log(this.selectedRadioOption);
    console.log(this.selectedCheckboxOptions);

    this.endDateTime = new Date().toUTCString();
    this.postChallenge();
  }

  private nextQuestion(): void {
    this.getNextChallenge();
  }

  showFinishTestConfirm() {
    this.confirmationModal.show(); 
  }

  private showConfirm(): void {
    //this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
    this.endDateTime = new Date().toUTCString();
    this.postChallenge(false);
  }


  private postChallenge(shouldGetNextQuestion: boolean = true): void {

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
            console.log("answer posted");
            if (shouldGetNextQuestion) {
                this.getNextChallenge();
            }
        }, reason => {
            console.log("answer posting failed");
        });
  }

  private finishAllChallenges(): void {
    //this.$timeout.cancel(this.myTimer);
    //this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
    this.endDateTime = new Date().toUTCString();
    this.postChallenge(false);
    console.log(this.currentTestId);
    this.challengeService.FinishAllChallenges(this.currentTestId).subscribe(
        response => {
            console.log("test finished");
            //this.showToast("Test finished");
            this.router.navigate(["../testFinished"], { relativeTo: this.activatedRoute});
        },
        error => {
            console.log(error);
        });;
    }
}
