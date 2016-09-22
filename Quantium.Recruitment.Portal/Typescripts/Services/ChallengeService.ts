/// <reference path="../viewmodels/challengeViewModel.ts" />

module Recruitment.Services {

    import ChallengeViewModel = Recruitment.ViewModels.ChallengeViewModel; 

    interface IChallengeService {
        getNextChallenge(): ng.IHttpPromise<any>;
        postChallenge(selectedItems: any): ng.IHttpPromise<any>;
    }

    export class ChallengeService implements IChallengeService {
        constructor(private $http: ng.IHttpService) {
        }

        public getNextChallenge(): ng.IHttpPromise<any> {
            return this.$http.get("/Temp/Provoke");
        }

        public postChallenge(selectedItems: any): ng.IHttpPromise<any> {
            return this.$http.post("http://localhost:60606/api/temp", selectedItems);
        }
    }
}
