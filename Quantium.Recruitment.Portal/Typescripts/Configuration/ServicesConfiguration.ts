/// <reference path="../services/challengeService.ts" />
/// <reference path="../services/testservice.ts" />

module Recruitment.Services {
    export class ServicesConfiguration {
        public static RegisterAll(app: ng.IModule): void {
            app.service("$challengeService", Recruitment.Services.ChallengeService);
            app.service("$testService", Recruitment.Services.TestService);
        }
    }
}