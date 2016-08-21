
module Recruitment.Controllers {
    interface ITestControllerScope extends ng.IScope {
        questionId: number;
        question: string;
        options: any;
        postQuestion: () => void;
        selection: any;
        toggleSelection: (employeeName: string) => void;
        //addSelection: (selectedOption: string) => void;
    }
    export class TestController {
        private selectedOptions: string[];

        constructor(private $scope: ITestControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService, private $challengeService: Recruitment.Services.ChallengeService) {
            this.getNextQuestion();
            this.$scope.postQuestion = () => this.postQuestion();
            //this.$scope.addSelection = (selectedOption) => this.addSelection(selectedOption);
            this.$scope.selection = [];
            this.$scope.toggleSelection = (employeeName) => this.toggleSelection(employeeName);

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

        private postQuestion(): void {

            this.$challengeService.postChallenge(this.$scope.selection)
                .then(result => {
                    this.$log.info("answer posted");
                }, reason => {
                    this.$log.error("answer posting failed");
                });

            this.getNextQuestion();
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

        private getNextQuestion(): any {
            this.$challengeService.getNextChallenge()
                .then(result => {
                    this.$scope.questionId = result.data.questionNumber;
                    this.$scope.question = result.data.questionText;
                    this.$scope.options = result.data.options;
                    this.$log.info("new question retrieved");
                    this.setTimer(result.data.questionTimeInSeconds);
                }, reason => {
                    this.$log.error("new question retrieval failed");

                });
        }
    }
}
