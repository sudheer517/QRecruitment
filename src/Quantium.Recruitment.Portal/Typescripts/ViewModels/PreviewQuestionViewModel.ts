module Recruitment.ViewModels {

    export class PreviewQuestionViewModel {
        public questionNumber: number;
        public questionText: string;
        public options: PreviewQuestionOptionViewModel[];
        public questionTimeInSeconds: number;

        constructor() { };
    }
}