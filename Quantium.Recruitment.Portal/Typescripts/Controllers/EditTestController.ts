/// <reference path="../viewmodels/questionoptionviewmodel.ts" />
/// <reference path="../viewmodels/questionviewmodel.ts" />

module Recruitment.Controllers {

    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;

    interface IEditTestControllerScope extends ng.IScope {
        selectedItem: string;
        changeTestName: ($event: any) => void;
        tests: Array<any>;
        saveChanges: () => void;
        selectedTestId: number;
        testData: TestDto;
    }

    export class EditTestController {

        constructor(private $scope: IEditTestControllerScope, private $log: ng.ILogService, private $testService: Recruitment.Services.TestService) {
            this.$scope.selectedItem = "Select a test";
            this.$scope.changeTestName = ($event: any) => this.changeTestName($event);
            this.$scope.tests = [{ name: "Test1", Id: 1 }, { name: "Test2", Id: 2 }, { name: "Test3", Id: 3  }]
            this.$scope.saveChanges = () => this.saveChanges();
        }

        public changeTestName($event: any): void {
            this.$scope.selectedItem = $event.target.innerText;
            this.$scope.selectedTestId = this.$scope.tests.filter(item => item.name === this.$scope.selectedItem)[0].Id;
            this.getTest();
        }

        public getTest() {
            var test = this.$testService.getTest(this.$scope.selectedTestId).then(
                response => {
                    this.$scope.testData = response.data;
                },
                error => { this.$log.error("error while retrieving test"); });
        }

        public saveChanges(): void {
            var savedTestData = this.$scope.testData;
        }
    }
}