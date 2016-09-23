/// <reference path="../typings/lodash/lodash.d.ts" />

module Recruitment {
    'use strict';
    export class AppBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ui.router",
                "ngFileUpload",
                "xeditable"
                 // for making text editable on edit test page
                //"mockingApp" // Can remove this when backend development is finished
            ]);

            Controllers.ControllersConfiguration.RegisterAll(this.app);
            Services.ServicesConfiguration.RegisterAll(this.app);
            Routes.RouteConfiguration.RegisterAll(this.app);

            //setting bootstrap theme for editable input directive xeditable
            //this.app.run((editableOptions) => { editableOptions.theme = 'bs3'; });
            this.app.run(($rootScope: any, $state: angular.ui.IStateService, $authService: Services.AuthService, editableOptions) => {

                editableOptions.theme = 'bs3';

                $rootScope.$on('$stateChangeSuccess', (event, toState, toParams, fromState, fromParams) => {

                    if (toState.name == "superAdmin") {
                        if ($authService.getRole() !== "SuperAdmin") {
                            $state.go(toState.data.redirectTo);
                        }
                    }
                    //if (!$authService.isAuthorized()) {
                    //    if ($authService.getMemorizedState() && (!_.has(fromState, 'data.redirectTo') || toState.name !== fromState.data.redirectTo)) {
                    //        $authService.clear();
                    //    }
                    //    if (_.has(toState, 'data.authorization') && _.has(toState, 'data.redirectTo')) {
                    //        if (_.has(toState, 'data.memory')) {
                    //            $authService.setMemorizedState(toState.name);
                    //        }
                    //        $state.go(toState.data.redirectTo);
                    //    }
                    //}

                });

                $rootScope.onLogout = function () {
                    $authService.clear();
                    $state.go('home');
                };
            });
        }

        public start() {
            $(document).ready(() => {
                console.log("booting " + this.app.name);
                angular.bootstrap(document, [this.app.name]);
            });
        }
    }
    new Recruitment.AppBuilder("recruitmentApp").start();
}