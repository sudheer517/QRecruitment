
module Recruitment.Controllers {

    interface IChallengeControllerScope extends ng.IScope {
        questions: Recruitment.ViewModels.ChallengeViewModel;
        getQuestions(): void;
    }

    export class ChallengeController {
        constructor(private $scope: IChallengeControllerScope, private $challengeService: Recruitment.Services.ChallengeService) {
            this.$scope.getQuestions = () => this.getQuestions();
        }

        private getQuestions(): void {
            this.$challengeService.getNextChallenge().then(response => {
                this.$scope.questions = response.data;
            });
        }
    }
}