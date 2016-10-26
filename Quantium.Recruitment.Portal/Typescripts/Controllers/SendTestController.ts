
module Recruitment.Controllers {
    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto;

    interface ICreateTestControllerScope extends ng.IScope {
        candidates: CandidateDto[];
        selectedtestOptions: SelectedTestOptions;
        testOptions: any;
        sendTest: () => void;
        testGenerationResult: string;
    }

    class SelectedTestOptions {
        public candidateIds: boolean[];

        constructor() { };
    }

    export class SendTestController {

        constructor(
            private $scope: ICreateTestControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private $jobService: Recruitment.Services.JobService,
            private $candidateService: Recruitment.Services.CandidateService) {
            this.getCandidates();
            this.$scope.selectedtestOptions = new SelectedTestOptions();
            this.$scope.sendTest = () => this.sendTest();
        }

        private getCandidates(): void {
            this.$candidateService.getAllCandidates().then(
                response => {
                    this.$scope.candidates = response.data;
                },
                error => {
                    this.$log.error("Candidates retrieval failed");
                }
            );
        }

        private sendTest(): void {
        }
    }
}
