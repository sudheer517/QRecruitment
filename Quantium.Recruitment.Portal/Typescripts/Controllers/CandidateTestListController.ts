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


        selected: any;
        query: any;
        limitOptions: any;
        options: any;
        finishedTests: any;
        getTypes: any;
        refreshTestData: () => void;

        archiveSelectedTests: () => void;
        gotoTest: (testId: number) => void;
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


            this.$scope.refreshTestData = () => this.getAllFinishedTests();
            this.$scope.selected = [];
            this.$scope.query = {
                order: 'candidateName',
                limit: 5,
                page: 1
            };
            this.$scope.limitOptions = [5, 10, 15];
            this.$scope.options = {
                rowSelection: true,
                multiSelect: true,
                autoSelect: false,
                decapitate: false,
                largeEditDialog: false,
                boundaryLinks: true,
                limitSelect: true,
                pageSelect: true
            };

            this.$scope.archiveSelectedTests = () => this.archiveSelectedTests();
            this.$scope.gotoTest = (testId) => this.gotoTest(testId);
        }

        private archiveSelectedTests(): void {
            var selectedTestIds: number[] = [];

            _.each(this.$scope.selected, (item, index) => {
                selectedTestIds.push(item.testId);
            });

            if (selectedTestIds.length > 0) {
                this.$testService.archiveTests(selectedTestIds).then(success => {
                        this.getAllFinishedTests();
                });
            }

            this.$scope.selected = [];
        }

        private gotoTest(testId: number) {
            var allTests = this.$scope.tests;
            var test: TestDto = allTests.filter(t => t.Id === testId)[0];
            this.getTestDetails(test);
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private getAllFinishedTests(): void {
            this.$testService.getFinishedTests().then(
                success => {
                    this.$scope.tests = success.data;
                    var allTestsData = [];
                    var testCount = this.$scope.tests.length;

                    _.each(this.$scope.tests, (testItem, itemIndex) => {
                        var testData = {
                            "candidateName": testItem.Candidate.FirstName + " " + testItem.Candidate.LastName,
                            "jobName": testItem.Job.Title,
                            "candidateEmail": testItem.Candidate.Email,
                            "finishedDate": moment(moment.utc(testItem.FinishedDate).toDate()).format('DD-MMM-YYYY hh:mm:ss A'),
                            "testResult": testItem.IsTestPassed ? "Passed" : "Failed",
                            "correctAnswers": { "value": testItem.TotalRightAnswers },
                            "college": testItem.Candidate.College,
                            "testId": testItem.Id
                        }

                        allTestsData.push(testData);
                    });

                    this.$scope.finishedTests = {
                        "count": testCount,
                        "data": allTestsData
                    };
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
