module Recruitment.Controllers {
    export interface DashboardControllerScope extends ng.IScope {
        toggleSidenav(): void;
    }
    export class DashboardController {

        constructor(private $scope: DashboardControllerScope, private $mdSidenav: ng.material.ISidenavService) {
            this.$scope.toggleSidenav = () => this.toggleSidenav();
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

    }
}
