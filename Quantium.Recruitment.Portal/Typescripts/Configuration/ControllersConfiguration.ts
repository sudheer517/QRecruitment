/// <reference path="../controllers/firstcontroller.ts" />
/// <reference path="../controllers/challengecontroller.ts" />
/// <reference path="../controllers/superadmincontroller.ts" />
/// <reference path="../controllers/createtestcontroller.ts" />
/// <reference path="../controllers/testcontroller.ts" />
/// <reference path="../controllers/uploadquestionscontroller.ts" />
/// <reference path="../controllers/addCandidatesController.ts" />
/// <reference path="../controllers/candidatedetailscontroller.ts" />
/// <reference path="../controllers/sendTestController.ts" />
/// <reference path="../controllers/testresultscontroller.ts" />
/// <reference path="../controllers/candidatetestlistcontroller.ts" />
/// <reference path="../controllers/dashboardcontroller.ts" />
/// <reference path="../controllers/candidatehomecontroller.ts" />

module Recruitment.Controllers {
    export class ControllersConfiguration {
        public static RegisterAll(app: ng.IModule): void {
            app.controller("firstController", Controllers.FirstController);
            app.controller("challengeController", Controllers.ChallengeController);
            app.controller("superAdminController", Controllers.SuperAdminController);
            app.controller("createTestController", Controllers.CreateTestController);
            app.controller("testController", Controllers.TestController);
            app.controller("uploadQuestionsController", Controllers.UploadQuestionsController);
            app.controller("addCandidatesController", Controllers.AddCandidatesController);
            app.controller("candidateDetailsController", Controllers.CandidateDetailsController);
            app.controller("sendTestController", Controllers.SendTestController);
            app.controller("testResultsController", Controllers.TestResultsController);
            app.controller("candidateTestListController", Controllers.CandidateTestListController);
            app.controller("dashboardController", Controllers.CandidateTestListController);
            app.controller("candidateHomeController", Controllers.CandidateHomeController);
        }
    }
}