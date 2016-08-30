module Recruitment.ViewModels {

    export class QuestionViewModel {
        public questionId: number;
        public questionText: string;
        public options: QuestionOptionViewModel[];
        public questionTimeInSeconds: number;

        constructor() { };
    }
}