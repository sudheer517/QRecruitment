
module Recruitment.Services {

    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto; 

    interface ICandidateService {
        getAllCandidates(): ng.IHttpPromise<CandidateDto[]>;
    }

    export class CandidateService implements ICandidateService {
        

        constructor(private $http: ng.IHttpService, private $q: ng.IQService) {
        }

        public getAllCandidates(): ng.IHttpPromise<CandidateDto[]> {
            return this.$http.get("/Candidate/GetAllCandidates");
        }

        public isInformationFilled(): ng.IPromise<boolean> {

            var deferred = this.$q.defer <boolean>();
            var promise = deferred.promise;
            this.$http.get("/Candidate/IsInformationFilled").then(
                success => {
                    data = Boolean(success.data);
                },
                error => {
                    console.log(error);
                });

            deferred.resolve();

            return deferred.promise;;
        }

        public fillCandidateInformation(candidateDto: CandidateDto): ng.IHttpPromise<boolean> {
            return this.$http.post("/Candidate/FillCandidateInformation", candidateDto);
        }
    }
}
