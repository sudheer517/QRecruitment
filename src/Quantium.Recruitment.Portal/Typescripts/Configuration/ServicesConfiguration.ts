
module Recruitment.Services {
    export class ServicesConfiguration {
        public static RegisterAll(app: ng.IModule): void {
            app.service("$questionService", Recruitment.Services.QuestionService);
        }
    }
}