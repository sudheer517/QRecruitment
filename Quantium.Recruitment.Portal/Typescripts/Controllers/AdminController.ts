/// <reference path="../typings/remoteservicesproxy.ts" />

module Recruitment.Controllers {

    import AdminDto = Quantium.Recruitment.ODataEntities.AdminDto;
    import DepartmentDto = Quantium.Recruitment.ODataEntities.DepartmentDto;

    interface IAdminControllerScope extends ng.IScope {
        saveChanges: () => void;
        admin: AdminDto;
        departmentList: DepartmentDto[];
        toggleSidenav(): void;
    }

    export class AdminController {

        constructor(private $scope: IAdminControllerScope,
            private $timeout: ng.ITimeoutService,
            private $adminService: Services.AdminService,
            private $departmentService: Services.DepartmentService,
            private $state: ng.ui.IStateService,
            private $mdToast: ng.material.IToastService,
            private $mdDialog: ng.material.IDialogService,
            private $mdSidenav: ng.material.ISidenavService) {

            this.getDepartmentList();
            //this.getAdminList();
            this.$scope.admin = new Quantium.Recruitment.ODataEntities.AdminDto();
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.toggleSidenav = () => this.toggleSidenav();
            //this.$scope.edit = (admin: adminModel) => this.edit(admin);
            //this.$scope.delete = (admin: adminModel) => this.delete(admin);
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private showAdminCreationStatusDialog(): void {
            var dialogOptions: ng.material.IDialogOptions = {
                contentElement: '#uploadStatusModal',
                clickOutsideToClose: false,
                escapeToClose: false,
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

        public saveChanges(): void {
            this.showAdminCreationStatusDialog();
            this.$scope.admin.IsActive = true;

            this.$adminService.addAdmin(this.$scope.admin)
                .then(response => {
                    this.$mdDialog.cancel();
                    this.$timeout(() => {
                        this.$state.reload();
                    }, 500)
                    if (response.data.StatusCode == 409) {
                        this.showToast("Duplicate Email");
                    }
                    else {
                        this.$timeout(() => {
                            this.showToast("Admin added successfully");
                        }, 1000);
                    }
                }, error => {
                    this.showToast("Admin creation failed");
                    this.$timeout(() => {
                        this.$state.reload();
                    }, 1000)
                });
        }

        //public edit(admin: any): void {
        //    this.$scope.firstName = admin.firstName;
        //    this.$scope.lastName = admin.lastName;
        //    this.$scope.email = admin.email;
        //    this.$scope.mobile = admin.mobile;
        //    this.$scope.division = admin.departmentId;
        //    this.$scope.adminId = admin.id;
        //    this.$scope.submitText = "Update";
        //}

        //public delete(admin: any): void {
        //    this.$adminService.deleteAdmin(admin.id)
        //        .then(response => {
        //            this.getAdminList();
        //            alert(admin.FirstName + " deleted")
        //        }, error => {
        //             alert("Error in service request")
        //        });
        //}

        //private clear() {
        //    this.$scope.firstName = "";
        //    this.$scope.lastName = "";
        //    this.$scope.email = "";
        //    this.$scope.mobile = undefined;
        //    this.$scope.division = 1;
        //}

        //private getAdminList() {
        //    this.$adminService.getAdminList().then(response => { this.$scope.adminList = response.data }, error => { console.log("hey"); });
        //}

        private getDepartmentList() {
            this.$departmentService.getAllDepartments().then(response => { this.$scope.departmentList = response.data }, error => { console.log("department retrieval failed"); });
        }
    }
}