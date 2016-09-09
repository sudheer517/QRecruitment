
module Recruitment.Services {

    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;

    interface ITestService {
        getTest(testId: number): ng.IHttpPromise<TestDto>;
    }

    export class TestService implements ITestService {
        constructor(private $http: ng.IHttpService) {
        }

        public getTest(testId: number): ng.IHttpPromise<TestDto> {
            return this.$http.get("http://localhost:60606/odata/test(testId)");
        }

    }
}
