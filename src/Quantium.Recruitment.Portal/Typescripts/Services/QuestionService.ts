
module Recruitment.Services {
    interface IDataAccessService {
        getQuestionsList(): ng.IPromise<any>;
    }

    export class QuestionService implements IDataAccessService {
        constructor(private $http: ng.IHttpService) {
        }

        public getQuestionsList(): ng.IPromise<any> {
            return this.$http.get('http://localhost:60606/api/temp');
        }
    }
}
