/// <reference path="../controllers/firstcontroller.ts" />
/// <reference path="../controllers/challengecontroller.ts" />
/// <reference path="../controllers/superadmincontroller.ts" />
/// <reference path="../controllers/createtestcontroller.ts" />
/// <reference path="../controllers/testcontroller.ts" />

module Recruitment.Controllers {
    export class ControllersConfiguration {
        public static RegisterAll(app: ng.IModule): void {
            app.controller("firstController", Controllers.FirstController);
            app.controller("challengeController", Controllers.ChallengeController);
            app.controller("superAdminController", Controllers.SuperAdminController);
            app.controller("createTestController", Controllers.CreateTestController);
            app.controller("testController", Controllers.TestController);
        }
    }
}