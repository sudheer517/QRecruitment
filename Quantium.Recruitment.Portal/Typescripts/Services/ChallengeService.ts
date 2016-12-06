/// <reference path="../viewmodels/challengeViewModel.ts" />

module Recruitment.Services {

    import ChallengeViewModel = Recruitment.ViewModels.ChallengeViewModel; 

    interface IChallengeService {
    }

    export class ChallengeService implements IChallengeService {
        constructor(private $http: ng.IHttpService) {
        }
    }
}
