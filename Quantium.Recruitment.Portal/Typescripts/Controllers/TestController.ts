
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
        selectedQuestionOptions: SelectedQuestionOptions;
        status: string;
        nextQuestion: () => void;
        finishTest: () => void;
        logout: () => void;
        toggleSidenav(): void;
        questionGroupText: string;
        imageUrl: string;
        totalTestTime: string;
        remainingTestTime: string;
        isRadioQuestion: boolean;
        radioOptionToggle: () => void;
        selectedRadio: any;
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
        private currentTestId: number;

        constructor(
            private $scope: ITestControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private $interval: ng.IIntervalService,
            private $mdDialog: ng.material.IDialogService,
            private $state: ng.ui.IStateService,
            private $timeout: ng.ITimeoutService,
            private $testService: Recruitment.Services.TestService,
            private $mdToast: ng.material.IToastService,
            private $mdSidenav: ng.material.ISidenavService) {
            this.getNextChallenge();
            this.$scope.fillAndPostChallenge = () => this.fillAndPostChallenge();
            this.$scope.selection = [];
            this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
            this.$scope.nextQuestion = () => this.nextQuestion();
            this.$scope.finishTest = () => this.finishTest();
            this.$scope.logout = () => this.settingsBeforeLogout();
            this.$scope.toggleSidenav = () => this.toggleSidenav();
            this.$scope.radioOptionToggle = () => this.radioOptionToggle();
            this.$scope.selectedRadio = { };
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private radioOptionToggle(): void {
            var selectedOptionId: number = this.$scope.selectedRadio.option;
            this.$scope.selectedQuestionOptions.optionIds = [];
            this.$scope.selectedQuestionOptions.optionIds[selectedOptionId] = true;
        }

        private settingsBeforeLogout(): void {
            this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
            this.postChallenge(false);
            this.logout();
        }

        private logout(): void {
            this.$testService.logout().then(
                success => {
                    console.log("logoff success");
                },
                error => {
                    console.log("logoff failed");
                });
        }

        private finishTest(): void {
            this.$timeout.cancel(this.myTimer);
            this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
            this.postChallenge(false);
            this.$testService.finishTest(this.currentTestId).then(
                response => {
                    this.showToast("Test finished");
                    this.$state.go("candidateHome");
                },
                error => {
                    console.log(error);
                });;
        }

        private showToast(toastMessage: string): void {
            var toast = this.$mdToast.simple()
                .textContent(toastMessage)
                .action('Ok')
                .highlightAction(true)
                .highlightClass('md-accent')// Accent is used by default, this just demonstrates the usage.
                .position("top right")
                .hideDelay(4000);

            this.$mdToast.show(toast).then(response => {
                if (response == 'ok') {
                    this.$mdToast.hide();
                    //alert('You clicked the \'UNDO\' action.');
                }
            });
        }

        private fillAndPostChallenge() {
            this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
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
            this.endDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
            //this.postChallenge(false);
            var parentElement = angular.element("#timeUpModal");
            this.$mdDialog.show({
                    templateUrl: '/views/timeUpTemplate.html',
                    disableParentScroll: true,
                    escapeToClose: false,
                    clickOutsideToClose: false,
                    scope: this.$scope,
                    preserveScope: true,
                    parent: parentElement
                });
        }

        private getNextChallenge(): any {
            this.$testService.getNextChallenge()
                .then(result => {
                    if (result.data.Question === undefined && JSON.parse(_.toString(result.data)) === "Finished") {
                        this.$state.go("candidateHome");
                        return;
                    }
                    this.currentTestId = result.data.TestId;
                    this.$timeout.cancel(this.myTimer);
                    this.myTimer = this.$timeout(() => { this.showConfirm() }, (result.data.Question.TimeInSeconds * 1000));
                    this.startDateTime = moment().utc().format("YYYY-MM-DD hh:mm:ss.SSS");
                    this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
                    this.currentChallenge = result.data;
                    this.$scope.imageUrl = result.data.Question.ImageUrl;
                    var questionGroup = result.data.Question.QuestionGroup;
                    this.$scope.questionGroupText = questionGroup ? questionGroup.Description : "";
                    this.$scope.challengeId = result.data.Question.Id;
                    this.$scope.questionText = result.data.Question.Text;
                    this.$scope.options = result.data.Question.Options;
                    this.$scope.currentChallenge = result.data.currentChallenge;
                    this.$scope.remainingChallenges = result.data.RemainingChallenges;
                    this.$scope.challengesAnswered = result.data.ChallengesAnswered;
                    this.$scope.totalTestTime = result.data.TotalTestTimeInMinutes;
                    this.$scope.remainingTestTime = result.data.RemainingTestTimeInMinutes;
                    this.$scope.isRadioQuestion = result.data.Question.IsRadio;
                    this.$log.info("new question retrieved");
                    this.setTimer(result.data.Question.TimeInSeconds);
                }, reason => {
                    this.$log.error("new question retrieval failed");
                });
        }
    }
}
