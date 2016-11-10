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
        toggleSidenav(): void;
    }

    export class UploadQuestionsController {

        constructor(private $scope: IUploadQuestionsControllerScope,
            private $log: ng.ILogService, private $http: ng.IHttpService,
            private Upload: ng.angularFileUpload.IUploadService,
            private $timeout: ng.ITimeoutService,
            private $mdDialog: ng.material.IDialogService,
            private $mdToast: ng.material.IToastService,
            private $state: ng.ui.IStateService,
            private $mdSidenav: ng.material.ISidenavService) {
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.previewQuestions = () => this.previewQuestions();
            this.$scope.previewQuestionModels = [];
            this.$scope.showPrerenderedDialog = (event) => this.showPrerenderedDialog();
            this.$scope.toggleSidenav = () => this.toggleSidenav();
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private showPrerenderedDialog(): void {
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
                .hideDelay(3000);

            this.$mdToast.show(toast).then(response => {
                if (response == 'ok') {
                    this.$mdToast.hide();
                    //alert('You clicked the \'UNDO\' action.');
                }
            });
        }

        public uploadFile(uploadUrl : string): void {
            this.showUploadStatusDialog();
            var file: any = this.$scope.files01[0].lfFile;
            if (file) {
                file.upload = this.Upload.upload({
                    url: uploadUrl,
                    data: { file: file },
                    method: 'POST',
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                });

                file.upload.then(response => {
                    this.$mdDialog.hide();
                    this.$timeout(() => {
                        this.$scope.previewQuestionModels = response.data;
                        this.showToast("Questions uploaded successfully");
                        this.$timeout(() => {
                            this.$state.go("dashboard");
                        }, 1000);
                    }, 1000);
                }, error => {
                    if (error.status > 0)
                        this.showToast(error.status + ': ' + error.data);
                }, evt => {
                    file.progress = Math.min(100, parseInt((100.0 * evt.loaded / evt.total) + ''));
                });
            }
        }

        public uploadFileAndPreview(uploadUrl: string): void {
            this.showUploadStatusDialog();
            var file: any = this.$scope.files01[0].lfFile;
            if (file) {
                file.upload = this.Upload.upload({
                    url: uploadUrl,
                    data: { file: file },
                    method: 'POST',
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                });

                file.upload.then(response => {
                    this.$mdDialog.hide();
                    this.$scope.previewQuestionModels = response.data;
                    this.showPrerenderedDialog();
                }, error => {
                    if (error.status > 0)
                        this.showToast(error.status + ': ' + error.data);
                }, evt => {
                    file.progress = Math.min(100, parseInt((100.0 * evt.loaded / evt.total) + ''));
                });
            }
        }

        public previewQuestions(): void {
            this.uploadFileAndPreview('/Question/PreviewQuestions');
        }

        public saveChanges(): void {
            this.uploadFile("/Question/AddQuestions");
        }
    }
}