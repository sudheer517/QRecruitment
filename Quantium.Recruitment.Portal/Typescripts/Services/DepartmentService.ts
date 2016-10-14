
module Recruitment.Services {

    import DepartmentDto = Quantium.Recruitment.ODataEntities.DepartmentDto;

    interface IDepartmentService {
        getAllDepartments(): ng.IHttpPromise<DepartmentDto[]>;
    }

    export class DepartmentService implements IDepartmentService {
        constructor(private $http: ng.IHttpService) {
        }

        public getAllDepartments(): ng.IHttpPromise<DepartmentDto[]> {
            return this.$http.get("/Department/GetAll");
        }
    }
}
