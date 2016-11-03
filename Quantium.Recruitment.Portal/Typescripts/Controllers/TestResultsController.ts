module Recruitment.Controllers {

    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;
    import OptionDto = Quantium.Recruitment.ODataEntities.OptionDto;
    import ChallengeDto = Quantium.Recruitment.ODataEntities.ChallengeDto;
    export interface ITestResultsControllerScope extends ng.IScope {
        test: TestDto;
        hasCandidateSelected: (option: OptionDto, currentChallenge: ChallengeDto) => boolean;
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
            this.$scope.hasCandidateSelected = (option, currentChallenge) => this.hasCandidateSelected(option, currentChallenge);
            //this.getAllFinishedTests();
            //this.$scope.getTestDetails = (selectedTest) => this.getTestDetails(selectedTest);
            //console.log("brah");
            //console.log(this.$scope);
            //console.log(this.$stateParams["selectedTest"]);
            //console.log(this.$state.params);
        }

        private hasCandidateSelected(option: OptionDto, currentChallenge: ChallengeDto): boolean {
            var test = this.$scope.test;
            if (currentChallenge.CandidateSelectedOptions.filter(cso => cso.OptionId === option.Id).length == 1)
                return true;

            return false;
        }

        private getTestById(testId: number): void {
            this.$testService.getFinishedTestById(testId).then(
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
