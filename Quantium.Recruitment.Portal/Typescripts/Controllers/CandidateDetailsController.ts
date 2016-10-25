
module Recruitment.Controllers {
    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto;

    interface ICandidateDetailsScope extends ng.IScope {
        candidate: CandidateDto;
        proceed(): void;
    }

    export class CandidateDetailsController {
        constructor(
            private $scope: ICandidateDetailsScope,
            private $state: ng.ui.IStateService,
            private $http: ng.IHttpService,
            private $candidateService: Recruitment.Services.CandidateService,
            private $testService: Recruitment.Services.TestService) {
            this.$scope.candidate = new Quantium.Recruitment.ODataEntities.CandidateDto();
            this.$scope.proceed = () => this.proceed();
            this.isInformationFilled();
        }

        private isInformationFilled(): void {
            var result = this.$candidateService.isInformationFilled();
        }

        private hasActiveTest(): boolean {
            var hasActiveTest = this.$testService.hasActiveTestForCandidate().then(
                response => {
                    return Boolean(response.data);
                },
                error => {
                    console.log(error);
                });

            return false;
        }

        private fillCandidateInformation() {
            this.$candidateService.fillCandidateInformation(this.$scope.candidate).then(
                response => {
                    this.$state.go("instructions");
                    console.log(response);
                },
                error => {
                    console.log(error);
                });
        }

        private proceed(): void {
            this.fillCandidateInformation();
        }
    }
}