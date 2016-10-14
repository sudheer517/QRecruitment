
module Recruitment.Services {

    import DifficultyDto = Quantium.Recruitment.ODataEntities.DifficultyDto;

    interface IDifficultyService {
        getAllDifficulties(): ng.IHttpPromise<DifficultyDto[]>;
    }

    export class DifficultyService implements IDifficultyService {
        constructor(private $http: ng.IHttpService) {
        }

        public getAllDifficulties(): ng.IHttpPromise<DifficultyDto[]> {
            return this.$http.get("/Difficulty/GetAll");
        }
    }
}
