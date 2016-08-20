/// <reference path="../viewmodels/questionviewmodel.ts" />

module Recruitment.Services {

    import QuestionViewModel = Recruitment.ViewModels.QuestionViewModel;

    interface IDataAccessService {
        getQuestionsList(): ng.IHttpPromise<QuestionViewModel>;
    }

    export class QuestionService implements IDataAccessService {
        constructor(private $http: ng.IHttpService) {
        }

        public getQuestionsList(): ng.IHttpPromise<any> {
            return this.$http.get('http://localhost:60606/api/temp');
        }

        public getNextQuestion(): ng.IHttpPromise<QuestionViewModel> {
            return this.$http.get("http://localhost:60606/api/temp/");
        }
    }
}
