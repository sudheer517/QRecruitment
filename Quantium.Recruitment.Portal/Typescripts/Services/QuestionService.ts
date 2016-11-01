
module Recruitment.Services {

    import QuestionLabelDifficultyDto = Quantium.Recruitment.ODataEntities.Question_Difficulty_LabelDto;

    interface IQuestionService {
        getQuestionsByLabelAndDifficulty(): ng.IHttpPromise<QuestionLabelDifficultyDto[]>;
    }

    export class QuestionService implements IQuestionService {
        constructor(private $http: ng.IHttpService) {
        }

        public getQuestionsByLabelAndDifficulty(): ng.IHttpPromise<QuestionLabelDifficultyDto[]> {
            return this.$http.get("/Question/GetQuestionsByLabelAndDifficulty");
        }
    }
}
