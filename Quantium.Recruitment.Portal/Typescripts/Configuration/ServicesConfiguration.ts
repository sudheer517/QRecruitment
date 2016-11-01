/// <reference path="../services/challengeService.ts" />
/// <reference path="../services/testservice.ts" />
/// <reference path="../services/adminservice.ts" />
/// <reference path="../services/authservice.ts" />
/// <reference path="../services/jobservice.ts" />
/// <reference path="../services/connectionservice.ts" />
/// <reference path="../services/departmentservice.ts" />
/// <reference path="../services/labelservice.ts" />
/// <reference path="../services/difficultyservice.ts" />
/// <reference path="../services/candidateservice.ts" />
/// <reference path="../services/questionservice.ts" />

module Recruitment.Services {
    export class ServicesConfiguration {
        public static RegisterAll(app: ng.IModule): void {
            app.service("$challengeService", Recruitment.Services.ChallengeService);
            app.service("$testService", Recruitment.Services.TestService);
            app.service("$authService", Recruitment.Services.AuthService);
            app.service("$adminService", Recruitment.Services.AdminService);
            app.service("$jobService", Recruitment.Services.JobService);
            app.service("$connectionService", Recruitment.Services.ConnectionService);
            app.service("$departmentService", Recruitment.Services.DepartmentService);
            app.service("$labelService", Recruitment.Services.LabelService);
            app.service("$difficultyService", Recruitment.Services.DifficultyService);
            app.service("$candidateService", Recruitment.Services.CandidateService);
            app.service("$questionService", Recruitment.Services.QuestionService);
        }
    }
}