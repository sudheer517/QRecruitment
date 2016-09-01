module Recruitment.Controllers {
    export interface FirstControllerScope extends ng.IScope {
        title: string;
    }
    export class FirstController {

        constructor(private $scope: FirstControllerScope) {
            this.$scope.title = "First Page title";
        }

    }
}
