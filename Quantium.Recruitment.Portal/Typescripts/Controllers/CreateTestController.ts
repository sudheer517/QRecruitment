
module Recruitment.Controllers {
    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;

    interface ICreateTestControllerScope extends ng.IScope {
        jobs: JobDto[];
        selectedJobName: string;
        changeSelectedJob: (selectedJob: JobDto) => void;
        totalQuestionsInSelectedJob: number;
    }
    export class CreateTestController {

        constructor(private $scope: ICreateTestControllerScope, private $log: ng.ILogService, private $http: ng.IHttpService, private $jobService: Recruitment.Services.JobService) {
            this.getJobs();
            this.$scope.selectedJobName = "Select Job";
            this.$scope.changeSelectedJob = (selectedJob) => this.changeSelectedJob(selectedJob);
        }

        private changeSelectedJob(selectedJob: JobDto) {
            this.$scope.selectedJobName = selectedJob.Title;
            this.$scope.totalQuestionsInSelectedJob = selectedJob.JobDifficultyLabels === null ? 0 : selectedJob.JobDifficultyLabels.length;
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
    }
}