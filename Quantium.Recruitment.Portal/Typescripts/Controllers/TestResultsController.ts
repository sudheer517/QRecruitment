module Recruitment.Controllers {

    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;
    export interface ITestResultsControllerScope extends ng.IScope {
        
    }


    export class TestResultsController {

        constructor(
            private $scope: ITestResultsControllerScope,
            private $testService: Recruitment.Services.TestService,
            private $state: ng.ui.IStateService,
            private $stateParams: ng.ui.IStateParamsService) {
            console.log("goal");
            console.log(this.$stateParams);
            console.log(this.$state.params);
            //this.getAllFinishedTests();
            //this.$scope.getTestDetails = (selectedTest) => this.getTestDetails(selectedTest);
            //console.log("brah");
            //console.log(this.$scope);
            //console.log(this.$stateParams["selectedTest"]);
            //console.log(this.$state.params);
        }

        //private getAllFinishedTests(): void {
        //    this.$testService.getFinishedTests().then(
        //        success => {
        //            this.$scope.tests = success.data;
        //        },
        //        error => {
        //            console.log(error);
        //        });
        //}

        

    }
}
