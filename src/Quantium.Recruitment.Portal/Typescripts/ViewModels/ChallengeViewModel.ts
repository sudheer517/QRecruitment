module Recruitment.ViewModels {

    export class ChallengeViewModel {
        public questionNumber: number;
        public questionText: string;
        public options: OptionViewModel[];
        public questionTimeInSeconds: number;

        constructor(questionNumber: number, questionText: string, options: OptionViewModel[], questionTimeInSeconds: number) {
            this.questionNumber = questionNumber;
            this.questionText = questionText;
            this.options = options;
            this.questionTimeInSeconds = questionTimeInSeconds;
        }
    }
}