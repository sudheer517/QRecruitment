
module Recruitment.Services {

    import AdminDto = Quantium.Recruitment.ODataEntities.AdminDto;
    import DepartmentDto = Quantium.Recruitment.ODataEntities.DepartmentDto;

    interface IAdminService {
        getAdminList(): ng.IHttpPromise<AdminDto[]>;
    }

    export class AdminService implements IAdminService {
        constructor(private $state: angular.ui.IStateService, private $http: ng.IHttpService) {
        }

        public getAdminList(): ng.IHttpPromise<AdminDto[]> {
            return this.$http.get("/Admin/GetAdminList");
        }

        public addAdmin(adminDto: AdminDto): ng.IHttpPromise<any> {
            return this.$http.post("/Admin/AddAdmin", adminDto, {
                headers: { 'Content-Type': 'application/json' }
            });
        }

        public updateAdmin(adminDto: AdminDto): ng.IHttpPromise<any> {
            return this.$http.post("/Admin/UpdateAdmin", adminDto, {
                headers: { 'Content-Type': 'application/json' }
            });
        }

        public deleteAdmin(key: number): ng.IHttpPromise<any> {
            return this.$http.post("/Admin/DeleteAdmin", key);
        }

        public getDepartmentList(): ng.IHttpPromise<DepartmentDto[]> {
            return this.$http.get("/Admin/GetDepartmentList");
        }
    }
}