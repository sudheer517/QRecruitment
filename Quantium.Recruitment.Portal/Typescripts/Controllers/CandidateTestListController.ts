//declare module angular.ui {
//    export interface IStateParamsService { example?: string; }
//}

module Recruitment.Controllers {
    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;

    export interface ICandidateTestListControllerScope extends ng.IScope {
        tests: TestDto[];
        selectedTest: TestDto;
        logout(): void;
        getTestDetails(test: TestDto): void;
        toggleSidenav(): void;
    }

    export class CandidateTestListController {
        constructor(
            private $scope: ICandidateTestListControllerScope,
            private $testService: Recruitment.Services.TestService,
            private $state: ng.ui.IStateService,
            private $stateParams: ng.ui.IStateParamsService,
            private $mdSidenav: ng.material.ISidenavService) {
            this.getAllFinishedTests();
            this.$scope.getTestDetails = (selectedTest) => this.getTestDetails(selectedTest);
            this.$scope.toggleSidenav = () => this.toggleSidenav();
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private getAllFinishedTests(): void {
            this.$testService.getFinishedTests().then(
                success => {
                    this.$scope.tests = success.data;
                },
                error => {
                    console.log(error);
                });
        }

        private getTestDetails(selectedTest: TestDto) {
            this.$state.go("testResults", { 'selectedTest': selectedTest, 'selectedTestId': selectedTest.Id }, { location: "replace" });
        }

        public logout(): void{

        }

    }
}
