module Recruitment.ViewModels {

    export class QuestionViewModel {
        public questionId: number;
        public questionText: string;
        public options: QuestionOptionViewModel[];
        public questionTimeInSeconds: number;

        constructor();
        constructor(questionId?: number, questionText?: string, options?: QuestionOptionViewModel[], questionTimeInSeconds?: number);

        constructor(questionId?: number, questionText?: string, options?: QuestionOptionViewModel[], questionTimeInSeconds?: number) {
            this.questionId = questionId;
            this.questionText = questionText;
            this.options = options;
            this.questionTimeInSeconds = questionTimeInSeconds;
        }
    }
}