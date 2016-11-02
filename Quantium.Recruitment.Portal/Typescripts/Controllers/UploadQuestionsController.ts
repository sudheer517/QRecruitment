/// <reference path="../typings/remoteservicesproxy.ts" />

module Recruitment.Controllers {

    import Question = Quantium.Recruitment.ODataEntities.QuestionDto;
    import QuestionGroup = Quantium.Recruitment.ODataEntities.QuestionGroupDto;
    import Option = Quantium.Recruitment.ODataEntities.OptionDto;
    import Label = Quantium.Recruitment.ODataEntities.LabelDto;
    import Difficulty = Quantium.Recruitment.ODataEntities.DifficultyDto;

    interface IUploadQuestionsControllerScope extends ng.IScope {
        fileUploadObj: any;
        uploadedFile: any;
        fileName: string;
        saveChanges: () => void;
        previewQuestions: () => void;
        previewQuestionModels: Question[];
        files01: any;
        showPrerenderedDialog: (event: any) => void;
    }

    export class UploadQuestionsController {

        constructor(private $scope: IUploadQuestionsControllerScope,
            private $log: ng.ILogService, private $http: ng.IHttpService,
            private Upload: ng.angularFileUpload.IUploadService,
            private $timeout: ng.ITimeoutService,
            private $mdDialog: ng.material.IDialogService,
            private $mdToast: ng.material.IToastService,
            private $state: ng.ui.IStateService) {
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.previewQuestions = () => this.previewQuestions();
            this.$scope.previewQuestionModels = [];
            this.$scope.showPrerenderedDialog = (event) => this.showPrerenderedDialog(event);
        }

        private showPrerenderedDialog(ev: any): void {
            this.previewQuestions();
            var dialogOptions: ng.material.IDialogOptions = {
                contentElement: '#myModal',
                clickOutsideToClose: true,
                scope: this.$scope,
                preserveScope: true,
                fullscreen: true
            };

            this.$mdDialog.show(dialogOptions);
        }

        private showUploadStatusDialog(): void {
            var dialogOptions: ng.material.IDialogOptions = {
                contentElement: '#uploadStatusModal',
                clickOutsideToClose: false,
                escapeToClose : false,
                scope: this.$scope,
                preserveScope: true,
            };

            this.$mdDialog.show(dialogOptions);
        }

        private showToast(toastMessage: string): void {
            var toast = this.$mdToast.simple()
                .textContent(toastMessage)
                .action('Ok')
                .highlightAction(true)
                .highlightClass('md-accent')// Accent is used by default, this just demonstrates the usage.
                .position("top right")
                .hideDelay(4000);

            this.$mdToast.show(toast).then(response => {
                if (response == 'ok') {
                    this.$mdToast.hide();
                    //alert('You clicked the \'UNDO\' action.');
                }
            });
        }

        public uploadFile(): void {
            this.showUploadStatusDialog();
            var file: any = this.$scope.files01[0].lfFile;
            if (file) {
                file.upload = this.Upload.upload({
                    url: '/Question/AddQuestions',
                    data: { file: file },
                    method: 'POST',
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                });

                file.upload.then(response => {
                    this.$mdDialog.hide();
                    this.$timeout(() => {
                        file.result = response.data;
                        this.showToast("Questions uploaded successfully");
                        this.$timeout(() => {
                            this.$state.go("dashboard");
                        }, 2000);
                    }, 1000);
                }, error => {
                    if (error.status > 0)
                        this.showToast(error.status + ': ' + error.data);
                }, evt => {
                    file.progress = Math.min(100, parseInt((100.0 * evt.loaded / evt.total) + ''));
                });
            }
        }

        public selectFile(file: any, errFiles: any) {
            this.$scope.fileName = file.name;
            this.$scope.previewQuestionModels = [];
        }

        public previewQuestions(): void {
            this.$scope.previewQuestionModels = [];
            var file = this.$scope.files01[0].lfFile;
            if (file && this.validateFormat(file)) {
                var fileReader = new FileReader();
                fileReader.readAsText(file);

                fileReader.onload = (event: any) => {
                    var csv = event.target.result;
                    this.processData(csv);
                }
            }
            else {
                this.showToast("Please upload csv file");
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
                previewQuestionModel.RandomizeOptions = columns[12] === "TRUE" ? true : false;
                previewQuestionModel.ImageUrl = columns[13];
                previewQuestionModel.Label = new Label(null, columns[10]);
                previewQuestionModel.Difficulty = new Difficulty(null, columns[11]);
                previewQuestionModel.QuestionGroup = new QuestionGroup(null, columns[14]);
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
            var file: any = this.$scope.files01[0].lfFile;
            var fileReader = new FileReader();

            if (this.validateFormat(file)) {
                this.uploadFile();
            }
            else {
                this.showToast("Please upload csv file");
            }
        }

        private validateFormat(file: any): boolean {
            return /^.+\.(csv)$/.test(file.name);
        }
    }
}