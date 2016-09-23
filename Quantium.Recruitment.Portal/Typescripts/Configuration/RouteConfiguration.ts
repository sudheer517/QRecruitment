
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
                //$urlRouterProvider.otherwise('/superAdmin');
                $urlRouterProvider.otherwise('/');
                //$stateProvider
                //    .state("superAdmin",
                //    {
                //        url: "/superAdmin", controller: Controllers.SuperAdminController, templateUrl: "/views/superAdminPage.html"
                //    })

                //    .state("createTest",
                //    {
                //        url: "/createTest", controller: Controllers.CreateTestController, templateUrl: "/views/createTestPage.html"
                //    })

                //    .state("uploadQuestions",
                //    {
                //        url: "/uploadQuestions", controller: Controllers.UploadQuestionsController, templateUrl: "/views/uploadQuestions.html"
                //    })

                //    .state("editTest",
                //    {
                //        url: "/editTest", controller: Controllers.EditTestController, templateUrl: "/views/editTest1.html"
                //    })

                //    .state("sendTest",
                //    {
                //        url: "/sendTest", controller: Controllers.CreateTestController, templateUrl: "/views/sendTestPage.html"
                //    })

                //    .state("addTeamAdmin",
                //    {
                //        url: "/addTeamAdmin", controller: Controllers.CreateTestController, templateUrl: "/views/addTeamAdminPage.html"
                //    })

                //    .state("addCandidates",
                //    {
                //        url: "/addCandidates", controller: Controllers.AddCandidatesController, templateUrl: "/views/addCandidatesPage.html"
                //    })

                //    .state("test",
                //    {
                //        url: "/test", controller: Controllers.TestController, templateUrl: "/views/testPage.html"
                //    })


                $stateProvider
                    .state('home', {
                        url: '/',
                        template: '<h1>Home</h1>'
                    })
                    .state("login", {
                        url: "/login",
                        template: '<button ng-click="onLogin()">Login</button>',
                        controller: ($scope, $state, $authService) => {
                            $scope.onLogin = function () {
                                $authService.go('private');
                            };
                        }
                    })
                    .state('private', {
                        url: '/private',
                        template: '<h1>Private</h1>',
                        data: {
                            authorization: true,
                            redirectTo: 'login'
                        }
                    })
                    .state('secret', {
                        url: '/secret',
                        template: '<h1>Secret</h1>',
                        data: {
                            authorization: true,
                            redirectTo: 'login',
                            memory: true
                        }
                    });

            });
        }
    }
}