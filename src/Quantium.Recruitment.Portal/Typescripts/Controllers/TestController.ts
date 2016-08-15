
module Recruitment.Controllers {
    interface ITestControllerScope extends ng.IScope {
        question: string;
        options: any;
        postQuestion: () => void;
        selection: any;
        toggleSelection: (employeeName: string) => void;
        //addSelection: (selectedOption: string) => void;
    }
    export class TestController {
        private counter: number = 1;
        private selectedOptions: string[];

        constructor(private $scope: ITestControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService) {
            this.getNextQuestion();
            this.$scope.postQuestion = () => this.postQuestion();
            //this.$scope.addSelection = (selectedOption) => this.addSelection(selectedOption);
            this.$scope.selection = [];
            this.$scope.toggleSelection = (employeeName) => this.toggleSelection(employeeName);

        }

        private toggleSelection(employeeName: string): void {
            //this.$scope.selection = [];
            // toggle selection for a given employee by name
            var idx = this.$scope.selection.indexOf(employeeName);

            // is currently selected
            if (idx > -1) {
                this.$scope.selection.splice(idx, 1);
            }

            // is newly selected
            else {
                this.$scope.selection.push(employeeName);
            }
        }

        private postQuestion(): void {
            
            this.$http.post('http://localhost:60606/api/temp', this.$scope.selection)
                .then(result => {
                    this.$log.info("answer posted");
                }, reason => {
                    this.$log.error("answer posting failed");
                });

            this.getNextQuestion();
        }

        private getNextQuestion(): any {
            this.$http.get('http://localhost:60606/api/temp/' + this.counter)
                .then(result => {
                    var questionData = <any>result.data;
                    this.$scope.question = questionData.questionText;
                    this.$scope.options = questionData.options;
                    this.$log.info("new question retrieved");
                    this.counter++;
                }, reason => {
                    this.$log.error("new question retrieval failed");

                });
        }
    }
}
