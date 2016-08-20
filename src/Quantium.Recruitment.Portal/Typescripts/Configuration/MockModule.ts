
module Mock {
    'use strict';
    export class MockModuleBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ngMockE2E",
            ]);

            this.app.run(($httpBackend: ng.IHttpBackendService) => {
                var challenge = {
                    questionText: "Who are you man?",
                    options: [
                        { optionId: 25, optionText: "Elf" }, { optionId: 26, optionText: "Muggle" }, { optionId: 27, optionText: "Wizard" }, { optionId: 28,optionText: "Nazgul" }
                    ]
                };

                $httpBackend.whenGET("http://localhost:60606/api/temp/").respond(challenge);
                $httpBackend.whenGET("views/superAdminPage.html").passThrough();
                $httpBackend.whenGET("views/testPage.html").passThrough();
            });
        }

        public initializeBackend($httpBackend: ng.IHttpBackendService): void {
            
        }
    }

    new Mock.MockModuleBuilder("mockingApp");

}