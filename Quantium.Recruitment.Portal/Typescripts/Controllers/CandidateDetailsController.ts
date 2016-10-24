
module Recruitment.Controllers {

    interface ICandidateDetailsScope extends ng.IScope {
        proceed(): void;
    }

    export class CandidateDetailsController {
        constructor(
            private $scope: ICandidateDetailsScope,
            private $state: ng.ui.IStateService) {
            this.$scope.proceed = () => this.proceed();
        }

        private proceed(): void {
            this.$state.go("instructions");
        }
    }
}