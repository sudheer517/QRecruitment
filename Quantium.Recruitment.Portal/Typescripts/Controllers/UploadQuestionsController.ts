/// <reference path="../viewmodels/questionoptionviewmodel.ts" />
/// <reference path="../viewmodels/questionviewmodel.ts" />

module Recruitment.Controllers {

    import PreviewQuestionModel = Recruitment.ViewModels.QuestionViewModel;
    import PreviewOptionModel = Recruitment.ViewModels.QuestionOptionViewModel;

    interface IUploadQuestionsControllerScope extends ng.IScope {
        selectedItem: string;
        changeTestName: ($event: any) => void;
        tests: any;
        fileUploadObj: any;
        selectFile: (file: any, errFiles: any) => void;
        uploadedFile: any;
        errorMsg: string;
        fileName: string;
        saveChanges: () => void;
        previewQuestions: () => void;
        previewQuestionModels: PreviewQuestionModel[];
    }

    export class UploadQuestionsController {

        constructor(private $scope: IUploadQuestionsControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService, private Upload: ng.angularFileUpload.IUploadService, private $timeout: ng.ITimeoutService) {
            this.$scope.selectedItem = "Select a test";
            this.$scope.changeTestName = ($event: any) => this.changeTestName($event);
            this.$scope.tests = [{ name: "Test1" }, { name: "Test2" }, { name: "Test3" }]
            this.$scope.selectFile = (file, errFiles) => this.selectFile(file, errFiles);
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.previewQuestions = () => this.previewQuestions();
            this.$scope.previewQuestionModels = [];
        }

        public changeTestName($event: any): void {
            this.$scope.selectedItem = $event.target.innerText;
        }

        public uploadFile(): void {
            var file: any = this.$scope.uploadedFile;

            if (file) {
                file.upload = this.Upload.upload({
                    url: 'http://localhost:60606/api/question',
                    data: { file: file },
                    method: 'POST',
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                });

                file.upload.then(response => {
                    this.$timeout(() => {
                        file.result = response.data;
                    });
                }, error => {
                    if (error.status > 0)
                        this.$scope.errorMsg = error.status + ': ' + error.data;
                }, evt => {
                    file.progress = Math.min(100, parseInt((100.0 * evt.loaded / evt.total) + ''));
                });
            }
        }

        public selectFile(file: any, errFiles: any) {
            this.$scope.uploadedFile = file;
            this.$scope.fileName = file.name;
            this.$scope.previewQuestionModels = [];
        }

        public previewQuestions(): void {
            if (this.$scope.previewQuestionModels.length < 1) {
                var file = this.$scope.uploadedFile;
                if (file) {
                    var fileReader = new FileReader();
                    fileReader.readAsText(file);

                    fileReader.onload = (event: any) => {
                        var csv = event.target.result;
                        this.processData(csv);
                    }
                }
            }
        }

        public processData(csv: any): void {
            var allLines: string[] = csv.split(/\r|\n/);
            allLines = allLines.filter(line => line.length > 0);
            var supportedOptionCount = 6;
            var headers: string[] = allLines[0].split(",");
            var totalColumnCount = headers.length;

            for (var csvLine = 1; csvLine < allLines.length; csvLine++) {
                var columns: string[] = allLines[csvLine].split(",");
                var previewQuestionModel: PreviewQuestionModel = new PreviewQuestionModel();
                previewQuestionModel.questionId = Number(columns[0]);
                previewQuestionModel.questionText = columns[1];
                var selectedOptions: string[] = columns[2].split(";");
                previewQuestionModel.questionTimeInSeconds = Number(columns[3]);

                var options: PreviewOptionModel[] = [];

                for (var columnIndex = 4; columnIndex < totalColumnCount; columnIndex++) {
                    var option: PreviewOptionModel = new PreviewOptionModel();
                    option.optionText = columns[columnIndex];
                    option.isSelected = selectedOptions.indexOf(headers[columnIndex]) == -1 ? false : true;

                    options.push(option);
                }

                previewQuestionModel.options = options;
                this.$scope.previewQuestionModels.push(previewQuestionModel);
                this.$scope.$apply();
            }
        }

        public saveChanges(): void {
            //send previewQuestionModel to server
        }
    }
}