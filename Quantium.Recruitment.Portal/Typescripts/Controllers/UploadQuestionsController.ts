/// <reference path="../typings/remoteservicesproxy.ts" />

module Recruitment.Controllers {

    import Question = Quantium.Recruitment.ODataEntities.QuestionDto;
    import Option = Quantium.Recruitment.ODataEntities.OptionDto;

    interface IUploadQuestionsControllerScope extends ng.IScope {
        fileUploadObj: any;
        selectFile: (file: any, errFiles: any) => void;
        uploadedFile: any;
        errorMsg: string;
        fileName: string;
        saveChanges: () => void;
        previewQuestions: () => void;
        previewQuestionModels: Question[];
    }

    export class UploadQuestionsController {

        constructor(private $scope: IUploadQuestionsControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService, private Upload: ng.angularFileUpload.IUploadService, private $timeout: ng.ITimeoutService) {
            this.$scope.selectFile = (file, errFiles) => this.selectFile(file, errFiles);
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.previewQuestions = () => this.previewQuestions();
            this.$scope.previewQuestionModels = [];
        }

        public uploadFile(): void {
            var file: any = this.$scope.uploadedFile;

            if (file) {
                file.upload = this.Upload.upload({
                    url: '/Question/AddQuestions',
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
                var previewQuestionModel = new Question();
                previewQuestionModel.Id = Number(columns[0]);
                previewQuestionModel.Text = columns[1];
                var selectedOptions: string[] = columns[2].split(";");
                previewQuestionModel.TimeInSeconds = Number(columns[3]);
                previewQuestionModel.RandomizeOptions = Boolean(columns[12]);
                previewQuestionModel.ImageUrl = columns[13];
                previewQuestionModel. 
                var options: Option[] = [];

                for (var columnIndex = 4; columnIndex < (supportedOptionCount + 4); columnIndex++) {
                    var option = new Option();
                    option.Text = columns[columnIndex];
                    option.IsAnswer = selectedOptions.indexOf(headers[columnIndex]) == -1 ? false : true;

                    if (!_.isEmpty(option.Text.trim())) {
                        options.push(option);
                    }
                }

                previewQuestionModel.Options = options;
                this.$scope.previewQuestionModels.push(previewQuestionModel);
                this.$scope.$apply();
            }
        }

        public saveChanges(): void {
            this.uploadFile();
        }
    }
}