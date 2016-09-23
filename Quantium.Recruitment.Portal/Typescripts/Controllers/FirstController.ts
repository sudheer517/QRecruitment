module Recruitment.Controllers {
    export interface FirstControllerScope extends ng.IScope {
        onLogout(): void;
    }
    export class FirstController {

        constructor(private $scope: FirstControllerScope) {
            this.$scope.onLogout = () => this.Logout();
        }

        public Logout(): void{

        }

    }
}
