module Recruitment.Controllers {
    export interface CandidateHomeControllerScope extends ng.IScope {
        candidateName: string;
    }
    export class CandidateHomeController {

        constructor(
            private $scope: CandidateHomeControllerScope,
            private $candidateService: Services.CandidateService) {
            this.getCandidateName();
        }

        private getCandidateName(): void {
            this.$candidateService.getCandidateName().then(
                success => {
                    this.$scope.candidateName = JSON.parse(success.data);
                },
                error => {
                    console.log("candidate name not found");
                });
        }
    }
}
