/// <reference path="../viewmodels/candiatesInfoViewModel.ts" />

module Recruitment.Controllers {

    import PreviewCandidatesModel = Quantium.Recruitment.ODataEntities.CandidateDto;

    interface ICandidatesControllerScope extends ng.IScope {
        candidatesArray: any;
        remove: (index: number) => void;
        add: () => void;
        saveCandidate: () => void;
        fileUploadObj: any;
        selectFile: (file: any, errFiles: any) => void;
        uploadedFile: any;
        fileName: string;
        saveChanges: () => void;
        previewCandidates: () => void;
        previewCandidatesModel: PreviewCandidatesModel[];
        showPrerenderedDialog: (event: any) => void;
        files01: any;
        validateEmail: any;
        toggleSidenav(): void;
    }

    export class AddCandidatesController {
        constructor(
            private $scope: ICandidatesControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private Upload: ng.angularFileUpload.IUploadService,
            private $timeout: ng.ITimeoutService,
            private $connectionService: Recruitment.Services.ConnectionService,
            private $candidateService: Recruitment.Services.CandidateService,
            private $state: ng.ui.IStateService,
            private $mdToast: ng.material.IToastService,
            private $mdDialog: ng.material.IDialogService,
            private $mdSidenav: ng.material.ISidenavService) {
            this.$scope.candidatesArray = {
                 candidates: []
            };
            this.$scope.remove = (index) => this.remove(index);
            this.$scope.add = () => this.add();
            this.$scope.saveCandidate = () => this.saveCandidate();
            this.$scope.selectFile = (file, errFiles) => this.selectFile(file, errFiles);
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.previewCandidates = () => this.previewCandidates();
            this.$scope.previewCandidatesModel = [];
            this.$scope.showPrerenderedDialog = (event) => this.showPrerenderedDialog(event);
            this.$scope.validateEmail = (input) => this.validateEmail(input);
            this.$scope.toggleSidenav = () => this.toggleSidenav();
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private showPrerenderedDialog(ev: any): void {
            this.previewCandidates();
            var dialogOptions: ng.material.IDialogOptions = {
                contentElement: '#myModal',
                clickOutsideToClose: true,
                scope: this.$scope,
                preserveScope: true,
                fullscreen: true
            };

            this.$mdDialog.show(dialogOptions);
        }
       public remove(index: number): void {
           this.$scope.candidatesArray.candidates.splice(index, 1);
        }

       private saveCandidate(): void {
           this.$candidateService.saveCandidate(this.$scope.candidatesArray.candidates).then(
               response => {
                   this.showToast("Candidate(s) added successfully");
                   this.$timeout(() => {
                       this.$state.go("dashboard");
                   }, 500);                 
                   console.log(response);
               },
               error => {
                   this.showToast(error);
                   console.log(error);
               });
       }

        public add(): void {
            this.$scope.candidatesArray.candidates.push({});
        }

        public uploadFile(file: any): void {
            if (file) {
                file.upload = this.Upload.upload({
                    url: '/Candidate/Add',
                    data: { file: file },
                    method: 'POST',
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                });

                file.upload.then(response => {
                    file.result = response.data;
                    this.showToast("Candidate(s) added successfully");
                    this.$timeout(() => {
                        this.$state.go("dashboard");
                    }, 500);
                }, error => {
                    if (error.status > 0)
                        this.showToast(error.status + ': ' + error.data);
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
            this.$scope.previewCandidatesModel = [];
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
                var candidateModel: PreviewCandidatesModel = new PreviewCandidatesModel();
                candidateModel.Id= Number(columns[0]);
                candidateModel.FirstName = columns[1];
                candidateModel.LastName = columns[2];
                candidateModel.Email = columns[3];

                this.$scope.previewCandidatesModel.push(candidateModel);
                this.$scope.$apply();
            }
        }

        public saveChanges(): void {
            var file: any = this.$scope.files01[0].lfFile;
            var fileReader = new FileReader();
            var isDataValidated = true;

            if (this.validateFormat(file)) {
                fileReader.readAsText(file);
                fileReader.onload = (event: any) => {
                    var csv = event.target.result;
                    var allLines: string[] = csv.split(/\r|\n/);
                    allLines = allLines.filter(line => line.length > 0);

                    for (var csvLine = 1; csvLine < allLines.length; csvLine++) {
                        var columns: string[] = allLines[csvLine].split(",");

                        if (!this.validateEmail(columns[3])) {
                            this.showToast("Email format is not correct, check preview for more details");
                            isDataValidated = false;
                        }
                    }

                    if (isDataValidated) {
                        this.uploadFile(file);
                    }
                }
            }
            else {
                this.showToast("Please upload csv file");
            }
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

        public validateEmail(input: string): boolean {
            return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(input);
        }

        private validateFormat(file: any): boolean {
            return /^.+\.(csv)$/.test(file.name);
        }
    }
}