
module Recruitment.Controllers {

    interface IChallengeControllerScope extends ng.IScope {
        questions: Recruitment.ViewModels.ChallengeViewModel;
    }

    export class ChallengeController {
        constructor(private $scope: IChallengeControllerScope, private $challengeService: Recruitment.Services.ChallengeService) {
        }
    }
}