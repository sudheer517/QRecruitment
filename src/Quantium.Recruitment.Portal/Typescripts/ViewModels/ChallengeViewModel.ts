module Recruitment.ViewModels {

    export class ChallengeViewModel {
        public questionNumber: number;
        public questionText: string;
        public options: ChallengeOptionViewModel[];
        public questionTimeInSeconds: number;

        constructor(questionNumber: number, questionText: string, options: ChallengeOptionViewModel[], questionTimeInSeconds: number) {
            this.questionNumber = questionNumber;
            this.questionText = questionText;
            this.options = options;
            this.questionTimeInSeconds = questionTimeInSeconds;
        }
    }
}