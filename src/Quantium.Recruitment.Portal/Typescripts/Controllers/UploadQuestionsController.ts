
module Recruitment.Controllers {

    interface IUploadQuestionsControllerScope extends ng.IScope {
        selectedItem: string;
        changeTestName: ($event: any) => void;
        tests: any;
    }

    export class UploadQuestionsController {

        constructor(private $scope: IUploadQuestionsControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService) {
            this.$scope.selectedItem = "Select a test";
            this.$scope.changeTestName = ($event: any) => this.changeTestName($event);
            this.$scope.tests = [{ name: "Test1" }, { name: "Test2" }, { name: "Test3" }]
        }

        public changeTestName($event: any): void {
            this.$scope.selectedItem = $event.target.innerText;
        }
        
    }
}