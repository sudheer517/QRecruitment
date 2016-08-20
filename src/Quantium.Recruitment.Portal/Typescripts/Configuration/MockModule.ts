/// <reference path="../viewmodels/optionviewmodel.ts" />
/// <reference path="../viewmodels/questionviewmodel.ts" />

module Mock {
    'use strict';
    import OptionViewModel = Recruitment.ViewModels.OptionViewModel;

    export class MockModuleBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ngMockE2E",
            ]);

            this.app.run(($httpBackend: ng.IHttpBackendService) => {

                var options: Recruitment.ViewModels.OptionViewModel[] = [];

                options.push(new OptionViewModel(25, "Elf"));
                options.push(new OptionViewModel(26, "Muggle"));
                options.push(new OptionViewModel(27, "Wizard"));
                options.push(new OptionViewModel(28, "Nazgul"));

                var challenge2: Recruitment.ViewModels.QuestionViewModel = new Recruitment.ViewModels.QuestionViewModel("Who are you man?", options);

                
                $httpBackend.whenGET("http://localhost:60606/api/temp/").respond(challenge2);
                $httpBackend.whenGET("views/superAdminPage.html").passThrough();
                $httpBackend.whenGET("views/testPage.html").passThrough();
            });
        }

        public initializeBackend($httpBackend: ng.IHttpBackendService): void {
            
        }
    }

    new Mock.MockModuleBuilder("mockingApp");

}