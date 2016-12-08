
module Recruitment.Services {

    import QuestionLabelDifficultyDto = Quantium.Recruitment.ODataEntities.Question_Difficulty_LabelDto;
    import QuestionDto = Quantium.Recruitment.ODataEntities.QuestionDto;

    interface IQuestionService {
        getQuestionsByLabelAndDifficulty(): ng.IHttpPromise<QuestionLabelDifficultyDto[]>;
        getAllQuestions(): ng.IHttpPromise<QuestionDto[]>;
    }

    export class QuestionService implements IQuestionService {
        constructor(private $http: ng.IHttpService) {
        }

        public getQuestionsByLabelAndDifficulty(): ng.IHttpPromise<QuestionLabelDifficultyDto[]> {
            return this.$http.get("/Question/GetQuestionsByLabelAndDifficulty");
        }

        public getAllQuestions(): ng.IHttpPromise<QuestionDto[]> {
            return this.$http.get("/Question/GetAllQuestions");
        }
    }
}
