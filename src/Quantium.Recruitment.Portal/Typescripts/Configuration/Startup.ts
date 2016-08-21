module Recruitment {
    'use strict';
    export class AppBuilder {
        app: ng.IModule;

        constructor(name: string) {
            this.app = angular.module(name, [
                "ui.router",
                "mockingApp" // Can remove this when backend development is finished
            ]);

            Controllers.ControllersConfiguration.RegisterAll(this.app);
            Services.ServicesConfiguration.RegisterAll(this.app);
            Routes.RouteConfiguration.RegisterAll(this.app);
            
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