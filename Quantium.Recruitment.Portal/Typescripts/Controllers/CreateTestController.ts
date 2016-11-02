
module Recruitment.Controllers {
    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;
    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto;
    import CandidateJobDto = Quantium.Recruitment.ODataEntities.Candidate_JobDto;

    interface ICreateTestControllerScope extends ng.IScope {
        jobs: JobDto[];
        candidates: CandidateDto[];
        changeSelectedJob: (selectedJob: JobDto) => void;
        totalQuestionsInSelectedJob: number;
        totalCorrectAnswersInSelectedJob: number;
        selectedJob: JobDto;
        selectedtestOptions: SelectedTestOptions;
        testOptions: any;
        generateTest: () => void;
        sendTest: () => void;
        testGenerationResult: boolean;
        hasSelectedAtleastOneCandidate: boolean;
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
            private $q: ng.IQService,
            private $jobService: Recruitment.Services.JobService,
            private $candidateService: Recruitment.Services.CandidateService,
            private $mdDialog: ng.material.IDialogService,
            private $mdToast: ng.material.IToastService,
            private $timeout: ng.ITimeoutService,
            private $state: ng.ui.IStateService) {
            this.getJobs();
            this.getCandidates();
            this.$scope.selectedtestOptions = new SelectedTestOptions();
            this.$scope.changeSelectedJob = (selectedJob) => this.changeSelectedJob(selectedJob);
            this.$scope.generateTest = () => this.generateTest();
            this.$scope.sendTest = () => this.sendTest();
            this.$scope.$watchCollection(() => this.$scope.selectedtestOptions.candidateIds, () => this.updateSelectedCandidateCount());
        }

        private updateSelectedCandidateCount(): void {
            
            var isSelected = false;
            _.each(this.$scope.selectedtestOptions.candidateIds, (item, index) => {
                if (item === true) {
                    isSelected = true;
                }
            });

            this.$scope.hasSelectedAtleastOneCandidate = isSelected;
        }

        private changeSelectedJob(selectedJob: JobDto) {
            this.$scope.selectedJob = selectedJob;
            var count = 0;
            var rightAnswersCount = 0;
            selectedJob.JobDifficultyLabels.forEach(item => {
                count += item.DisplayQuestionCount;
                rightAnswersCount += item.PassingQuestionCount;
            });
            this.$scope.totalQuestionsInSelectedJob = count;
            this.$scope.totalCorrectAnswersInSelectedJob = rightAnswersCount;

        }

        private showPrerenderedDialog(): void {
            var dialogOptions: ng.material.IDialogOptions = {
                contentElement: '#uploadStatusModal',
                clickOutsideToClose: false,
                escapeToClose: false,
                scope: this.$scope,
                preserveScope: true
            };

            this.$mdDialog.show(dialogOptions);
        }

        private showToast(toastMessage: string): void {
            var toast = this.$mdToast.simple()
                .textContent(toastMessage)
                .action('Ok')
                .highlightAction(true)
                .highlightClass('md-accent')// Accent is used by default, this just demonstrates the usage.
                .position("top right")
                .hideDelay(4000);

            this.$mdToast.show(toast).then(response => {
                if (response == 'ok') {
                    this.$mdToast.hide();
                }
            });
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
            this.showPrerenderedDialog();
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
                this.$mdDialog.hide();
                this.showToast("Tests generated successfully");
                this.$timeout(() => {
                    this.$state.go("dashboard");
                }, 1000);
                this.$scope.testGenerationResult = true;
            }, error => {
                this.$scope.testGenerationResult = false;
            });
        }

        private sendTest(): void {
            var candidates: CandidateDto[] = [];
            var candidateIds = this.$scope.selectedtestOptions.candidateIds;
            var getDataPromise = this.$candidateService.getAllCandidates().then(
                response => {
                    _.each(candidateIds, (item, index) => {
                        if (item === true) {
                            var candidate = new Quantium.Recruitment.ODataEntities.CandidateDto();
                            candidate = response.data[index - 1];
                            candidates.push(candidate);
                        }
                    })
                },
                error => {
                    this.$log.error("Candidates retrieval failed");
                }
            );
            this.$q.all([getDataPromise]).then(() => this.sendTestData(candidates));
        }

        private sendTestData(candidates: Quantium.Recruitment.ODataEntities.CandidateDto[]): void {
            this.$http.post("/CreateTest/SendTests", candidates).then(response => {
                console.log(response);
            }, error => {
                console.log(error);
            });
        }
    }
}