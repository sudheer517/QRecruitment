
module Recruitment.Controllers {
    import ChallengeDto = Quantium.Recruitment.ODataEntities.ChallengeDto;
    import CandidateSelectedOptionDto = Quantium.Recruitment.ODataEntities.CandidateSelectedOptionDto;

    interface ITestControllerScope extends ng.IScope {
        challengeId: number;
        questionText: string;
        options: any;
        postChallenge: () => void;
        selection: any;
        toggleSelection: (employeeName: string) => void;
        //addSelection: (selectedOption: string) => void;
        selectedQuestionOptions: SelectedQuestionOptions;
    }

    class SelectedQuestionOptions {
        public optionIds: boolean[];
        constructor() { }
    }

    export class TestController {
        private selectedOptions: string[];
        private currentChallenge: ChallengeDto;

        constructor(
            private $scope: ITestControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private $testService: Recruitment.Services.TestService) {
            this.getNextChallenge();
            this.$scope.postChallenge = () => this.postChallenge();
            //this.$scope.addSelection = (selectedOption) => this.addSelection(selectedOption);
            this.$scope.selection = [];
            this.$scope.toggleSelection = (employeeName) => this.toggleSelection(employeeName);
            this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
        }

        private toggleSelection(employeeName: string): void {
            //this.$scope.selection = [];
            // toggle selection for a given employee by name
            var idx = this.$scope.selection.indexOf(employeeName);

            // is currently selected
            if (idx > -1) {
                this.$scope.selection.splice(idx, 1);
            }

            // is newly selected
            else {
                this.$scope.selection.push(employeeName);
            }
        }

        private postChallenge(): void {
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

            challengeDto.AnsweredTime = "2016-10-22 18:26:18.133";
            challengeDto.CandidateSelectedOptions = candidateSelectedOptionDtoItems;
            //this.currentChallenge.CandidateSelectedOptions.push(
            //challengeDto.CandidateSelectedOptions.push(
            this.$testService.postChallenge(challengeDto)
                .then(result => {
                    this.$log.info("answer posted");
                    this.getNextChallenge();
                }, reason => {
                    this.$log.error("answer posting failed");
                });
        }

        private setTimer(questionTimeInSeconds: number) {
            var clock;
            var clockObj: any = $('.clock');

            clock = clockObj.FlipClock(questionTimeInSeconds, {
                clockFace: 'MinuteCounter',
                autoStart: false,
                callbacks: {
                    stop: function () {
                        $('.message').html('The clock has stopped!')
                    }
                }
            });

            clock.setCountdown(true);
            clock.start();
        }

        private getNextChallenge(): any {
            this.$testService.getNextChallenge()
                .then(result => {
                    this.$scope.selectedQuestionOptions = new SelectedQuestionOptions();
                    this.currentChallenge = result.data;
                    this.currentChallenge.StartTime = "2016-10-22 18:26:12.133";
                    this.$scope.challengeId = result.data.Question.Id;
                    this.$scope.questionText = result.data.Question.Text;
                    this.$scope.options = result.data.Question.Options;
                    this.$log.info("new question retrieved");
                    this.setTimer(result.data.Question.TimeInSeconds);
                }, reason => {
                    this.$log.error("new question retrieval failed");

                });
        }
    }
}
