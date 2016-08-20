module Recruitment.ViewModels {

    export class QuestionViewModel {
        public questionText: string;
        public options: OptionViewModel[];
        constructor(questionText: string, options: OptionViewModel[]) {
            this.questionText = questionText;
            this.options = options;
        }
    }
}