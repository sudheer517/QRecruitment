
module Recruitment.Services {

    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto; 

    interface ICandidateService {
        getAllCandidates(): ng.IHttpPromise<CandidateDto[]>;
    }

    export class CandidateService implements ICandidateService {
        constructor(private $http: ng.IHttpService) {
        }

        public getAllCandidates(): ng.IHttpPromise<CandidateDto[]> {
            return this.$http.get("/Candidate/GetAllCandidates");
        }
    }
}
