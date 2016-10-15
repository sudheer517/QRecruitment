
module Recruitment.Controllers {
    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;
    import DepartmentDto = Quantium.Recruitment.ODataEntities.DepartmentDto;
    import LabelDto = Quantium.Recruitment.ODataEntities.LabelDto;
    import DifficultyDto = Quantium.Recruitment.ODataEntities.DifficultyDto;
    import JobDifficultyLabelDto = Quantium.Recruitment.ODataEntities.Job_Difficulty_LabelDto;

    interface ICreateJobControllerScope extends ng.IScope {
        job: JobDto;
        departments: DepartmentDto[];
        labels: LabelDto[];
        difficulties: DifficultyDto[];
        selectedDepartment: DepartmentDto;
        selectedOptions: SelectedOptions;
        createJob(): void;
    }
    export class SelectedOptions {
        public labelIds: boolean[];
        public difficultyIds: number[];
        public questionCounts: number[];

        constructor() { };
    }
    export class CreateJobController {

        constructor(
            private $scope: ICreateJobControllerScope,
            private $log: ng.ILogService,
            private $http: ng.IHttpService,
            private $departmentService: Recruitment.Services.DepartmentService,
            private $labelService: Recruitment.Services.LabelService,
            private $difficultyService: Recruitment.Services.DifficultyService) {
            this.getDepartments();
            this.getLabels();
            this.getDifficulties();
            this.$scope.selectedOptions = new SelectedOptions();
            this.$scope.createJob = () => this.createJob();
        }

        private getDepartments(): void {
            this.$departmentService.getAllDepartments()
                .then(result => {
                    this.$scope.departments = result.data;
                }, error => {
                    this.$log.info('departments retrieval failed');
                    console.log(error);
                });
        }

        private getLabels(): void {
            this.$labelService.getAllLabels()
                .then(result => {
                    this.$scope.labels = result.data;
                }, error => {
                    this.$log.info('labels retrieval failed');
                    console.log(error);
                });
        }

        private getDifficulties(): void {
            this.$difficultyService.getAllDifficulties()
                .then(result => {
                    this.$scope.difficulties = result.data;
                }, error => {
                    this.$log.info('difficulties retrieval failed');
                    console.log(error);
                });
        }

        private createJob(): void {
            var job = this.$scope.job;
            var labelIds = this.$scope.selectedOptions.labelIds;
            var difficultyIds = this.$scope.selectedOptions.difficultyIds;
            job.JobDifficultyLabels = [];

            _.each(labelIds, (item, index) => {
                if (item === true) {
                    var jobDifficultyLabel = new JobDifficultyLabelDto();
                    jobDifficultyLabel.Label = new LabelDto(index);
                    jobDifficultyLabel.Difficulty = new DifficultyDto(difficultyIds[index]);
                    jobDifficultyLabel.QuestionCount = this.$scope.selectedOptions.questionCounts[index];
                    job.JobDifficultyLabels.push(jobDifficultyLabel);
                }
            });

            this.$http.post("/Job/Create", job).then(response => {
                console.log(response);
            }, error => {
                console.log(error);
            });
        }
    }
}