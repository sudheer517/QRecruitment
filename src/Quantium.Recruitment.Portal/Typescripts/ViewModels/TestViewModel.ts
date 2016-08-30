module Recruitment.ViewModels {

    export class TestViewModel {
        public testId: number;
        public testName: string;
        public questions: QuestionViewModel[];

        constructor();
        constructor(testId?: number, testName?: string, questions?: QuestionViewModel[]);

        constructor(testId?: number, testName?: string, questions?: QuestionViewModel[]) {
            this.testId = testId;
            this.testName = testName;
            this.questions = questions;
        }
    }
}