
module Recruitment.Services {
    interface IDataAccessService {
        getQuestionsList(): ng.IPromise<any>;
    }

    export class QuestionServiceConfig {
        public static Register(app: ng.IModule): void {
            app.factory("$questionService", ($http: ng.IHttpService) => {
                return new QuestionService($http);
            });
        }
    }

    export class QuestionService implements IDataAccessService {
        constructor(private $http: ng.IHttpService) {
        }

        public getQuestionsList(): ng.IPromise<any> {
            return this.$http.get('http://localhost:60606/api/temp');
        }
    }
}
