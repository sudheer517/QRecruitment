
module Recruitment.Services {

    import LabelDto = Quantium.Recruitment.ODataEntities.LabelDto;

    interface ILabelService {
        getAllLabels(): ng.IHttpPromise<LabelDto[]>;
    }

    export class LabelService implements ILabelService {
        constructor(private $http: ng.IHttpService) {
        }

        public getAllLabels(): ng.IHttpPromise<LabelDto[]> {
            return this.$http.get("/Label/GetAll");
        }
    }
}
