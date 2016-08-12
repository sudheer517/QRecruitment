/// <reference path="../controllers/firstcontroller.ts" />
/// <reference path="../controllers/questioncontroller.ts" />
/// <reference path="../controllers/superadmincontroller.ts" />
/// <reference path="../controllers/createtestcontroller.ts" />

module Recruitment {
    'use strict';
    export class AppBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ui.router"
            ]);

            this.app.controller("firstController", Controllers.FirstController);
            this.app.controller("questionsController", Controllers.QuestionController);
            this.app.controller("superAdminController", Controllers.SuperAdminController);
            this.app.controller("createTestController", Controllers.CreateTestController);

            this.app.config((
                $stateProvider: angular.ui.IStateProvider,
                $urlRouterProvider: angular.ui.IUrlRouterProvider,
                $httpProvider: ng.IHttpProvider) => {
                if (!$httpProvider.defaults.headers.get) {
                    $httpProvider.defaults.headers.get = {};
                }

                //disable IE ajax request caching
                $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
                // extra
                $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
                $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

                $urlRouterProvider.otherwise('/superAdmin');

                $stateProvider
                    .state("superAdmin",
                    {
                        url: "/superAdmin",
                        views: {
                            "super-admin-view": { controller: Controllers.SuperAdminController, templateUrl: "views/superAdminPage.html" }
                        }
                    })

                    .state("superAdmin.createTest",
                    {
                        url: "/createTest", controller: Controllers.CreateTestController, templateUrl: "views/createTestPage.html", 
                    })

                    //.state("firstState",
                    //{
                    //    url: "/index",
                    //    views: {
                    //        "main-view": { controller: "firstController", templateUrl: "views/first.html" },
                    //        "main-view2": { controller: "questionsController", templateUrl: "views/second.html" }
                    //    }
                    //})


                    //.state("secondState",
                    //{
                    //    url: "/second",
                    //    views: {
                    //        "main-view": { controller: "questionsController", templateUrl: "views/second.html" },
                    //    }
                    //});
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