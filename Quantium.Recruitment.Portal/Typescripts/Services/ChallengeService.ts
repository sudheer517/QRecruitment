/// <reference path="../viewmodels/challengeViewModel.ts" />

module Recruitment.Services {

    import ChallengeViewModel = Recruitment.ViewModels.ChallengeViewModel;

    interface IDataAccessService {
        getNextChallenge(): ng.IHttpPromise<ChallengeViewModel>;
        postChallenge(selectedItems: any): ng.IHttpPromise<any>;
    }

    export class ChallengeService implements IDataAccessService {
        constructor(private $http: ng.IHttpService) {
        }

        public getNextChallenge(): ng.IHttpPromise<ChallengeViewModel> {
            return this.$http.get("http://localhost:60606/api/temp");
        }

        public postChallenge(selectedItems: any): ng.IHttpPromise<any> {
            return this.$http.post("http://localhost:60606/api/temp", selectedItems);
        }
    }
}
