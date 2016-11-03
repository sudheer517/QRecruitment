/// <reference path="../typings/lodash/lodash.d.ts" />

module Recruitment {
    'use strict';
    export class AppBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ui.router",
                "ngFileUpload",
                "xeditable", // for making text editable on edit test page
                "ngMaterial",
                "ngMessages",
                "ngMdIcons",
                "lfNgMdFileInput"
                //"mockingApp" // Can remove this when backend development is finished
            ]);

            Controllers.ControllersConfiguration.RegisterAll(this.app);
            Services.ServicesConfiguration.RegisterAll(this.app);
            Routes.RouteConfiguration.RegisterAll(this.app);

            this.app.config((
                $mdIconProvider: ng.material.IIconProvider,
                $mdThemingProvider: ng.material.IThemingProvider) => {

                $mdIconProvider.icon("menu", "../img/menu.svg", 24);
                $mdIconProvider.icon("dashboard", "../img/dashboard.svg", 24);
                $mdIconProvider.icon("upload", "../img/upload.svg", 24);
                $mdIconProvider.icon("createJob", "../img/createJob.svg", 24);
                $mdIconProvider.icon("addCandidates", "../img/personAdd.svg", 24);
                $mdIconProvider.icon("createTest", "../img/create.svg", 24);
                $mdIconProvider.icon("editTest", "../img/editTest.svg", 24);
                $mdIconProvider.icon("sendTest", "../img/sendTest.svg", 24);
                $mdIconProvider.icon("addAdmin", "../img/addAdmin.svg", 24);
                $mdIconProvider.icon("takeTest", "../img/takeTest.svg", 24);
                $mdIconProvider.icon("close", "../img/close.svg", 24);
                $mdIconProvider.icon("add", "../img/add.svg", 24);
                $mdIconProvider.icon("print", "../img/print.svg", 24);
                $mdThemingProvider.theme('default').primaryPalette('red').accentPalette('blue');
                $mdThemingProvider.theme('dark-grey').backgroundPalette('grey').dark();
                $mdThemingProvider.theme('grey-variant').primaryPalette('grey').accentPalette('cyan');
                $mdThemingProvider.theme('deep-orange').primaryPalette('blue-grey').accentPalette('grey');
            });
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