
module Recruitment.Services {

    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;

    interface IJobService {
        getAllJobs(): ng.IHttpPromise<JobDto[]>;
    }

    export class JobService implements IJobService {
        constructor(private $http: ng.IHttpService) {
        }

        public getAllJobs(): ng.IHttpPromise<JobDto[]> {
            return this.$http.get("/Job/GetAll");
        }
    }
}
