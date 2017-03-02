﻿
module Recruitment.Routes {
    export class RouteConfiguration {
        public static RegisterAll(app: ng.IModule): void {
            app.config((
                $stateProvider: angular.ui.IStateProvider,
                $urlRouterProvider: angular.ui.IUrlRouterProvider,
                $httpProvider: ng.IHttpProvider) => {
                if (!$httpProvider.defaults.headers.get) {
                    $httpProvider.defaults.headers.get = {};
                }

                //disable IE ajax request caching
                $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
                $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
                $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

                //Default route
                //$urlRouterProvider.otherwise('/dashboard');
                //$urlRouterProvider.otherwise('/');
                $stateProvider
                    .state("superAdmin",
                    {
                        url: "/superAdmin", controller: Controllers.SuperAdminController, templateUrl: "/views/superAdminPage.html"
                        ,data: { role: "SuperAdmin" , redirectTo: "candidateDetails"}
                    })

                    .state("createTest",
                    {
                        url: "/createTest", controller: Controllers.CreateTestController, templateUrl: "/views/createTestPage.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("uploadQuestions",
                    {
                        url: "/uploadQuestions", controller: Controllers.UploadQuestionsController, templateUrl: "/views/uploadQuestions.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("editTest",
                    {
                        url: "/editTest", controller: Controllers.EditTestController, templateUrl: "/views/editTest1.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("sendTest",
                    {
                        url: "/sendTest", controller: Controllers.SendTestController, templateUrl: "/views/sendTestPage.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("addTeamAdmin",
                    {
                        url: "/addTeamAdmin", controller: Controllers.AdminController, templateUrl: "/views/addTeamAdminPage.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("addCandidates",
                    {
                        url: "/addCandidates", controller: Controllers.AddCandidatesController, templateUrl: "/views/addCandidatesPage.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("test",
                    {
                        url: "/test", controller: Controllers.TestController, templateUrl: "/views/testPage.html",
                        resolve: {
                            hasActiveTest: ($testService: Recruitment.Services.TestService, $state: ng.ui.IStateService) => {
                                return $testService.hasActiveTestForCandidate()
                                    .then(success => {
                                        if (String(success.data) === "false") {
                                            $state.go("candidateHome");
                                        }
                                    }, error => { console.log(error); return false; });
                            }
                        }
                    })

                    .state("instructions",
                    {
                        url: "/instructions", controller: Controllers.FirstController, templateUrl: "/views/instructions.html",
                        resolve: {
                            hasActiveTest: ($testService: Recruitment.Services.TestService, $state: ng.ui.IStateService) => {
                                return $testService.hasActiveTestForCandidate()
                                    .then(success => {
                                        if (String(success.data) === "false") {
                                            $state.go("candidateHome");
                                        }
                                    }, error => { console.log(error); return false; });
                            }
                        }
                    })

                    .state("createJob",
                    {
                        url: "/createJob", controller: Controllers.CreateJobController, templateUrl: "/views/createJob.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("dashboard",
                    {
                        url: "/dashboard", controller: Controllers.DashboardController, templateUrl: "/views/dashboard.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("candidateDetails",
                    {
                        url: "/candidateDetails",
                        controller: Controllers.CandidateDetailsController,
                        templateUrl: "/views/candidateDetails.html",
                        resolve: {
                            isInformationFilled: ($candidateService: Recruitment.Services.CandidateService, $state: ng.ui.IStateService) => {
                                return $candidateService.isInformationFilled()
                                    .then(success => {
                                        if (String(success.data) === "true") {
                                            $state.go("instructions");
                                        }                                        
                                    }, error => { console.log(error); return false });
                            }
                        }
                    })
                    .state("candidateNoTest",
                    {
                        url: "/candidateNoTest", controller: Controllers.CandidateHomeController, templateUrl:"/views/candidateNoTest.html"
                    })

                    .state("candidateHome",
                    {
                        url: "/candidateHome", controller: Controllers.CandidateHomeController, templateUrl: "/views/candidateHomePage.html",
                        resolve: {
                            hasActiveTest: ($testService: Recruitment.Services.TestService, $state: ng.ui.IStateService) => {
                                return $testService.hasActiveTestForCandidate()
                                    .then(success => {
                                        if (String(success.data) === "true") {
                                            $state.go("instructions");
                                        }
                                        else
                                            $state.go("candidateNoTest");
                                    }, error => { console.log(error); return false; });
                            }
                        }
                    })

                    .state("candidateTestList",
                    {
                        url: "/candidateTestList", controller: Controllers.CandidateTestListController, templateUrl: "/views/candidateTestListPage.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("testResults",
                    {
                        url: "/testResults/{selectedTestId}", controller: Controllers.TestResultsController, templateUrl: "/views/testResults.html", params: { 'selectedTest': null }
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })

                    .state("candidatesList",
                    {
                        url: "/candidatesList", controller: Controllers.CandidatesListController, templateUrl: "/views/candidatesList.html"
                        , data: { role: "SuperAdmin", redirectTo: "candidateDetails" }
                    })
            });
        }
    }
}