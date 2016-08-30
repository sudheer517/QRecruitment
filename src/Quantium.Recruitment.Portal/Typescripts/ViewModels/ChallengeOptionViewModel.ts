module Recruitment.ViewModels {

    export class ChallengeOptionViewModel {
        private optionId: number;
        private optionText: string;
        constructor(optionId: number, optionText: string) {
            this.optionId = optionId;
            this.optionText = optionText;
        }
    }
}