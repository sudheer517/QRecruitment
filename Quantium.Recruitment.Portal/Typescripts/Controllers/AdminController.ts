/// <reference path="../typings/remoteservicesproxy.ts" />

module Recruitment.Controllers {

    import adminModel = Quantium.Recruitment.ODataEntities.AdminDto;
    import departmentModel = Quantium.Recruitment.ODataEntities.DepartmentDto;

    interface IAdminControllerScope extends ng.IScope {
        errorMsg: string;
        saveChanges: () => void;
        edit: (admin: adminModel) => void;
        delete: (admin: adminModel) => void;
        adminList: adminModel[];
        departmentList: departmentModel[];
        submitText: string;
        adminId:number;
        firstName: string;
        lastName: string;
        email: string;
        mobile: number;
        division: number;
    }

    export class AdminController {

        constructor(private $scope: IAdminControllerScope,
            private $timeout: ng.ITimeoutService,
            private $adminService: Services.AdminService) {

            this.getDepartmentList();
            this.getAdminList();

            this.$scope.submitText = "Add";
            this.$scope.saveChanges = () => this.saveChanges();
            this.$scope.edit = (admin: adminModel) => this.edit(admin);
            this.$scope.delete = (admin: adminModel) => this.delete(admin);
        }

        public saveChanges(): void {
            var admin = new adminModel()
            admin.FirstName = this.$scope.firstName;
            admin.LastName = this.$scope.lastName;
            admin.Email = this.$scope.email;
            admin.Mobile = this.$scope.mobile;
            admin.DepartmentId = this.$scope.division;
            admin.IsActive = true;

            if (this.$scope.submitText === "Add") {
                this.$adminService.addAdmin(admin)
                    .then(response => {
                        this.clear();
                        this.getAdminList();
                        alert(admin.FirstName + " added !!")
                    }, error => {
                        alert("Error in service request")
                    });
            } else {
                admin.Id = this.$scope.adminId;
                this.$adminService.updateAdmin(admin)
                    .then(response => {
                        this.clear();
                        this.getAdminList();
                        alert(admin.FirstName + " detail updated !!")
                    }, error => {
                        alert("Error in service request")
                    });
            }
        }

        public edit(admin: any): void {
            this.$scope.firstName = admin.firstName;
            this.$scope.lastName = admin.lastName;
            this.$scope.email = admin.email;
            this.$scope.mobile = admin.mobile;
            this.$scope.division = admin.departmentId;
            this.$scope.adminId = admin.id;
            this.$scope.submitText = "Update";
        }

        public delete(admin: any): void {
            this.$adminService.deleteAdmin(admin.id)
                .then(response => {
                    this.getAdminList();
                    alert(admin.FirstName + " deleted")
                }, error => {
                     alert("Error in service request")
                });
        }

        private clear() {
            this.$scope.firstName = "";
            this.$scope.lastName = "";
            this.$scope.email = "";
            this.$scope.mobile = undefined;
            this.$scope.division = 1;
        }

        private getAdminList() {
            this.$adminService.getAdminList().then(response => { this.$scope.adminList = response.data }, error => { console.log("hey"); });
        }

        private getDepartmentList() {
            this.$adminService.getDepartmentList().then(response => { this.$scope.departmentList = response.data }, error => { console.log("hey"); });
        }
    }
}