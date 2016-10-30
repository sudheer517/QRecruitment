module Recruitment.Controllers {

    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;
    export interface ITestResultsControllerScope extends ng.IScope {
        test: TestDto;
    }

    export class TestResultsController {

        constructor(
            private $scope: ITestResultsControllerScope,
            private $testService: Recruitment.Services.TestService,
            private $state: ng.ui.IStateService) {
            var stateParamTest = <TestDto>this.$state.params['selectedTest'];
            if (!stateParamTest) {
                this.getTestById(this.$state.params['selectedTestId']);
            }
            else {
                this.$scope.test = stateParamTest;
            }
            //this.getAllFinishedTests();
            //this.$scope.getTestDetails = (selectedTest) => this.getTestDetails(selectedTest);
            //console.log("brah");
            //console.log(this.$scope);
            //console.log(this.$stateParams["selectedTest"]);
            //console.log(this.$state.params);
        }

        private getTestById(testId: number): void {
            this.$testService.getTestById(testId).then(
                success => {
                    this.$scope.test = success.data;
                    console.log(success.data);
                    _.each(success.data.Challenges, (item, index) => {
                        _.each(item.Question.Options, (optionItem, optionIndex) => {
                            if (optionItem.IsAnswer == true) {
                                console.log(item.Question.Text + '-' + item.Id);
                            }
                        });
                    });
                },
                error => {
                    console.log(error);
                });
        }

        

    }
}
