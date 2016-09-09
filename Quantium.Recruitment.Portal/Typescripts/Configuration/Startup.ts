module Recruitment {
    'use strict';
    export class AppBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ui.router",
                "ngFileUpload",
                "xeditable", // for making text editable on edit test page
                "mockingApp" // Can remove this when backend development is finished
            ]);

            Controllers.ControllersConfiguration.RegisterAll(this.app);
            Services.ServicesConfiguration.RegisterAll(this.app);
            Routes.RouteConfiguration.RegisterAll(this.app);

            //setting bootstrap theme for editable input directive xeditable
            this.app.run((editableOptions) => { editableOptions.theme = 'bs3'; });
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