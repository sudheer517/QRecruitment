
module Recruitment.Controllers {
    import ChallengeDto = Quantium.Recruitment.ODataEntities.ChallengeDto;
    import CandidateSelectedOptionDto = Quantium.Recruitment.ODataEntities.CandidateSelectedOptionDto;

    interface ITestControllerScope extends ng.IScope {
        challengeId: number;
        questionText: string;
        options: any;
        fillAndPostChallenge: () => void;
        selection: any;
        toggleSelection: (employeeName: string) => void;
        currentChallenge: number;
        remainingChallenges: number;
        challengesAnswered: Boolean[];
        //addSelection: (selectedOption: string) => void;
        selectedQuestionOptions: SelectedQuestionOptions;
        status: string;
        nextQuestion: () => void;
    }

    class SelectedQuestionOptions {
        public optionIds: boolean[];
        constructor() { }
    }

    export class TestController {
        private selectedOptions: string[];
        private currentChallenge: ChallengeDto;
        private startDateTime: string;
        private endDateTime: string;
        private clock: any;
        private myTimer: any;

        constructor(
            private $scope: ITestControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private $interval: ng.IIntervalService,
            private $mdDialog: ng.material.IDialogService,
            private $state: ng.ui.IStateService,
            private $timeout: ng.ITimeoutService,
            private $testService: Recruitment.Services.TestService) {
            this.getNextChallenge();
            this.$scope.fillAndPostChallenge = () => this.fillAndPostChallenge();
            this.$scope.selection = [];
            this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
            this.$scope.nextQuestion = () => this.nextQuestion();
        }

        private fillAndPostChallenge() {
            this.endDateTime = moment().format("YYYY-MM-DD hh:mm:ss.SSS");
            this.postChallenge();
        }

        private postChallenge(shouldGetNextQuestion: boolean = true): void {
            this.currentChallenge.StartTime = this.startDateTime;
            this.currentChallenge.AnsweredTime = this.endDateTime;
            var challengeDto = this.currentChallenge;
            var candidateSelectedOptionDtoItems: CandidateSelectedOptionDto[] = [];
            var selectedOptionIds = this.$scope.selectedQuestionOptions.optionIds;
            challengeDto.QuestionId = challengeDto.Question.Id;
            _.each(selectedOptionIds, (item, index) => {
                if (item === true) {
                    var candidateSelectedOptionDto = new Quantium.Recruitment.ODataEntities.CandidateSelectedOptionDto();
                    candidateSelectedOptionDto.ChallengeId = this.currentChallenge.Id;
                    candidateSelectedOptionDto.OptionId = index;
                    candidateSelectedOptionDtoItems.push(candidateSelectedOptionDto);
                }
            });

            challengeDto.CandidateSelectedOptions = candidateSelectedOptionDtoItems;
            
            this.$testService.postChallenge(challengeDto)
                .then(result => {
                    this.$log.info("answer posted");
                    if (shouldGetNextQuestion) {
                        this.getNextChallenge();
                    }
                }, reason => {
                    this.$log.error("answer posting failed");
                });
        }

        private setTimer(questionTimeInSeconds: number) {
            var clockObj: any = $('.clock');
            this.clock = clockObj.FlipClock(questionTimeInSeconds, {
                clockFace: 'MinuteCounter',
                autoStart: false,
                callbacks: {
                    stop: () => {
                        this.$timeout.cancel();
                    }
                }
            });

            this.clock.setCountdown(true);
            this.clock.start();
        }

        private nextQuestion(): void {
            this.$mdDialog.hide();
            this.getNextChallenge();
        }

        private showConfirm(): void {
            this.endDateTime = moment().format("YYYY-MM-DD hh:mm:ss.SSS");
            this.postChallenge(false);
            this.$mdDialog.show({
                    templateUrl: '/views/timeUpTemplate.html',
                    disableParentScroll: true,
                    escapeToClose: false,
                    clickOutsideToClose: false,
                    scope: this.$scope,
                    preserveScope: true
                });
        }

        private getNextChallenge(): any {
            this.$testService.getNextChallenge()
                .then(result => {
                    if (result.data.Question === undefined && JSON.parse(_.toString(result.data)) === "Finished") {
                        this.$state.go("candidateHome");
                        return;
                    }
                    this.$timeout.cancel(this.myTimer);
                    this.myTimer = this.$timeout(() => { this.showConfirm() }, (result.data.Question.TimeInSeconds * 1000));
                    this.startDateTime = moment().format("YYYY-MM-DD hh:mm:ss.SSS");
                    this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
                    this.currentChallenge = result.data;
                    this.$scope.challengeId = result.data.Question.Id;
                    this.$scope.questionText = result.data.Question.Text;
                    this.$scope.options = result.data.Question.Options;
                    this.$scope.currentChallenge = result.data.currentChallenge;
                    this.$scope.remainingChallenges = result.data.RemainingChallenges;
                    this.$scope.challengesAnswered = result.data.ChallengesAnswered;
                    this.$log.info("new question retrieved");
                    this.setTimer(result.data.Question.TimeInSeconds);
                }, reason => {
                    this.$log.error("new question retrieval failed");
                });
        }
    }
}
