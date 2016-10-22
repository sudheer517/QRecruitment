
module Recruitment.Controllers {
    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;
    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto;
    import CandidateJobDto = Quantium.Recruitment.ODataEntities.Candidate_JobDto;

    interface ICreateTestControllerScope extends ng.IScope {
        jobs: JobDto[];
        candidates: CandidateDto[];
        selectedJobName: string;
        changeSelectedJob: (selectedJob: JobDto) => void;
        totalQuestionsInSelectedJob: number;
        selectedJob: JobDto;
        selectedtestOptions: SelectedTestOptions;
        testOptions: any;
        generateTest: () => void;
        testGenerationResult: string;
    }
    class SelectedTestOptions {
        public candidateIds: boolean[];
        constructor() { };
    }

    export class CreateTestController {

        constructor(
            private $scope: ICreateTestControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private $jobService: Recruitment.Services.JobService,
            private $candidateService: Recruitment.Services.CandidateService) {
            this.getJobs();
            this.getCandidates();
            this.$scope.selectedtestOptions = new SelectedTestOptions();
            this.$scope.selectedJobName = "Select Job";
            this.$scope.changeSelectedJob = (selectedJob) => this.changeSelectedJob(selectedJob);
            this.$scope.generateTest = () => this.generateTest();
        }

        private changeSelectedJob(selectedJob: JobDto) {
            this.$scope.selectedJob = selectedJob;
            this.$scope.selectedJobName = selectedJob.Title;
            var count = 0;
            selectedJob.JobDifficultyLabels.forEach(item => count += item.QuestionCount)
            this.$scope.totalQuestionsInSelectedJob = count;
        }

        private getJobs(): void {
            this.$jobService.getAllJobs()
                .then(result => {
                    this.$scope.jobs = result.data;
                }, error => {
                    this.$log.info('jobs retrieval failed');
                    console.log(error);
                });
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

        private generateTest(): void {
            var candidateIds = this.$scope.selectedtestOptions.candidateIds;
            var candidatesJobs: CandidateJobDto[] = [];

            _.each(candidateIds, (item, index) => {
                if (item === true) {
                    var candidateJob = new Quantium.Recruitment.ODataEntities.Candidate_JobDto();
                    candidateJob.Candidate = new Quantium.Recruitment.ODataEntities.CandidateDto(index);
                    candidateJob.Job = new Quantium.Recruitment.ODataEntities.JobDto(this.$scope.selectedJob.Id);
                    candidatesJobs.push(candidateJob);
                }
            });

            this.$http.post("/CreateTest/GenerateTests", candidatesJobs).then(response => {
                this.$scope.testGenerationResult = "Tests generated successfully";
                console.log(response);
            }, error => {
                this.$scope.testGenerationResult = "Tests generation failed";
                console.log(error);
            });
        }
    }
}