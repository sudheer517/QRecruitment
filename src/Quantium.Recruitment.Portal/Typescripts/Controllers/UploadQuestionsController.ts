module Recruitment.Controllers {

    interface IUploadQuestionsControllerScope extends ng.IScope {
        selectedItem: string;
        changeTestName: ($event: any) => void;
        tests: any;
        //uploadFile: (files: any) => void;
        upload: any;
        fileUploadObj: any;
        uploadFiles: (file: any, errFiles: any) => void;
        f: any;
        errFile: any;
        errorMsg: any;
    }

    export class UploadQuestionsController {

        constructor(private $scope: IUploadQuestionsControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService, private Upload: ng.angularFileUpload.IUploadService, private $timeout: ng.ITimeoutService) {
            this.$scope.selectedItem = "Select a test";
            this.$scope.changeTestName = ($event: any) => this.changeTestName($event);
            this.$scope.tests = [{ name: "Test1" }, { name: "Test2" }, { name: "Test3" }]
            this.$scope.upload = [];
            this.$scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };
            this.$scope.uploadFiles = (file, errFiles) => this.uploadFiles(file, errFiles);
        }

        public changeTestName($event: any): void {
            this.$scope.selectedItem = $event.target.innerText;
        }

        public uploadFiles(file: any, errFiles: any) {
            this.$scope.f = file;
            this.$scope.errFile = errFiles && errFiles[0];
            if (file) {
                file.upload = this.Upload.upload({
                    url: 'https://angular-file-upload-cors-srv.appspot.com/upload',
                    data: { file: file },
                    method: 'POST'
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
    }
}