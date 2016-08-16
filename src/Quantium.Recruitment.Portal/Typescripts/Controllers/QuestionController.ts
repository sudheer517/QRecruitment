
module Recruitment.Controllers {

    interface IQuestionControllerScope extends ng.IScope {
        questions: any[];
        getQuestions(): void;
    }

    export class QuestionController {
        constructor(private $scope: IQuestionControllerScope, private $questionService: Recruitment.Services.QuestionService) {
            this.$scope.getQuestions = () => this.getQuestions();
        }

        private getQuestions(): void {
            this.$questionService.getQuestionsList().then(response => {
                this.$scope.questions = response.data;
            });
        }
    }
}