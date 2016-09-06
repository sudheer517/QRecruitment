/// <reference path="../viewmodels/candiatesInfoViewModel.ts" />
/// <reference path="../typings/odataentities.d.ts" />

module Recruitment.Controllers {

    import PreviewCandidatesModel = Recruitment.ViewModels.CandiatesInfoViewModel;;

    interface ICandidatesControllerScope extends ng.IScope {
        candidatesArray: any;
        remove: (index: number) => void;
        add: () => void;
        fileUploadObj: any;
        selectFile: (file: any, errFiles: any) => void;
        uploadedFile: any;
        errorMsg: string;
        fileName: string;
        saveChanges: () => void;
        previewCandidates: () => void;
        previewCandidatesModel: PreviewCandidatesModel[];
}

    export class AddCandidatesController {

        constructor(private $scope: ICandidatesControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService, private Upload: ng.angularFileUpload.IUploadService, private $timeout: ng.ITimeoutService) {
            this.$scope.candidatesArray = {
                 candidates: []
            };
            this.$scope.remove = (index) => this.remove(index);
            this.$scope.add = () => this.add();
            this.$scope.selectFile = (file, errFiles) => this.selectFile(file, errFiles);
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.previewCandidates = () => this.previewCandidates();
            this.$scope.previewCandidatesModel = [];
        }

       public remove(index: number): void {
           this.$scope.candidatesArray.candidates.splice(index, 1);
       }

        public add(): void {
            this.$scope.candidatesArray.candidates.push({});
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
            this.$scope.previewCandidatesModel = [];
        }

        public previewCandidates(): void {
            if (this.$scope.previewCandidatesModel.length < 1) {
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
                var questionModel: PreviewCandidatesModel = new PreviewCandidatesModel();
                questionModel.id = Number(columns[0]);
                questionModel.name = columns[1];
                questionModel.email = columns[2];

                this.$scope.previewCandidatesModel.push(questionModel);
                this.$scope.$apply();
            }

            
        }

        public saveChanges(): void {
            var randomObj = new Quantium.Recruitment.ApiServices.Models.DepartmentDto();
            randomObj.Name = "Hello";
        }
    }
}