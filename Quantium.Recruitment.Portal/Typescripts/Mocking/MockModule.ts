
module Mock {
    'use strict';
    
    export class MockModuleBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ngMockE2E",
            ]);

            this.app.run(($httpBackend: ng.IHttpBackendService) => {
                $httpBackend.whenGET("http://localhost:60606/api/temp").respond(() => {
                    return [200, Mocks.ChallengeMock.getNextQuestion()];
                });

                $httpBackend.whenPOST("http://localhost:60606/api/temp").respond(() => {
                    return [200, { randomStuff: "could be anything here" }];
                });


                //Edit Test mock
                $httpBackend.whenGET("http://localhost:60606/odata/test(1)").respond(() => {
                    return [200, Mocks.EditTestMock.getTestData(1)];
                });

                $httpBackend.whenGET("http://localhost:60606/odata/test(2)").respond(() => {
                    return [200, Mocks.EditTestMock.getTestData(2)];
                });

                $httpBackend.whenGET("http://localhost:60606/odata/test(3)").respond(() => {
                    return [200, Mocks.EditTestMock.getTestData(1)];
                });
                

                $httpBackend.whenGET("views/superAdminPage.html").passThrough();
                $httpBackend.whenGET("views/createTestPage.html").passThrough();
                $httpBackend.whenGET("views/uploadQuestions.html").passThrough();
                $httpBackend.whenGET("views/editTest1.html").passThrough();
                $httpBackend.whenGET("views/sendTestPage.html").passThrough();
                $httpBackend.whenGET("views/addTeamAdminPage.html").passThrough();
                $httpBackend.whenGET("views/addCandidatesPage.html").passThrough();
                $httpBackend.whenGET("views/testPage.html").passThrough();
                
                $httpBackend.resetExpectations();
            });
        }

        
    }

    new Mock.MockModuleBuilder("mockingApp");
}