module Recruitment.ViewModels {

    export class QuestionOptionViewModel {
        public optionText: string
        public isSelected: boolean;

        constructor();
        constructor(optionText?: string, isSelected?: boolean);

        constructor(optionText?: string, isSelected?: boolean) {
            this.optionText = optionText;
            this.isSelected = isSelected;
        }
    }
}