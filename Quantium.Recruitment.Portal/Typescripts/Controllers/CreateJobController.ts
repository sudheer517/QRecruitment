
module Recruitment.Controllers {
    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;
    import DepartmentDto = Quantium.Recruitment.ODataEntities.DepartmentDto;
    import LabelDto = Quantium.Recruitment.ODataEntities.LabelDto;
    import DifficultyDto = Quantium.Recruitment.ODataEntities.DifficultyDto;
    import JobDifficultyLabelDto = Quantium.Recruitment.ODataEntities.Job_Difficulty_LabelDto;
    import QuestionDifficultyLabelDto = Quantium.Recruitment.ODataEntities.Question_Difficulty_LabelDto;

    interface ICreateJobControllerScope extends ng.IScope {
        job: JobDto;
        departments: DepartmentDto[];
        labels: LabelDto[];
        difficulties: DifficultyDto[];
        selectedDepartment: DepartmentDto;
        selectedOptions: SelectedOptions;
        createJob(): void;
        jobDifficultyLabelArray: any;
        removeJobDifficultyLabel: (index: number) => void;
        addJobDifficultyLabel(): void;
        questionDifficultyLabels: QuestionDifficultyLabelDto[];
        showAvailableQuestions(jobDifficultyLabel: any): void;
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
            private $departmentService: Services.DepartmentService,
            private $labelService: Services.LabelService,
            private $difficultyService: Services.DifficultyService,
            private $mdDialog: ng.material.IDialogService,
            private $state: ng.ui.IStateService,
            private $mdToast: ng.material.IToastService,
            private $questionService: Services.QuestionService) {
            this.getDepartments();
            this.getLabels();
            this.getDifficulties();
            this.$scope.selectedOptions = new SelectedOptions();
            this.$scope.createJob = () => this.createJob();
            this.$scope.jobDifficultyLabelArray = {
                jobDifficultyLabels: []
            };
            this.$scope.removeJobDifficultyLabel = (index) => this.removeJobDifficultyLabel(index);
            this.$scope.addJobDifficultyLabel = () => this.addJobDifficultyLabel();
            this.getQuestionDifficultyLabels();
            this.$scope.showAvailableQuestions = (jobDifficultyLabel: any) => this.showAvailableQuestions(jobDifficultyLabel);
            //this.$scope.jobDifficultyLabelArray.QuestionCount = this.$scope.questionDifficultyLabels.filter((qdlDto, index) => {
            //    qdlDto.LabelId = 
            //});

            //_.each(this.$scope.jobDifficultyLabelArray, (item, index) => {
            //    item.QuestionCount = _.each(this.$scope.questionDifficultyLabels, (qdlItem, qdlIndex) => {
            //        if (item.LabelId === qdlItem.LabelId && item.DifficultyId === qdlItem.DifficultyId)
            //            return qdlItem.QuestionCount;
            //        else
            //            return "";
            //    });
            //});
        }

        private showAvailableQuestions(jobDifficultyLabel: any): void {
            _.each(this.$scope.questionDifficultyLabels, (qdlItem, qdlIndex) => {
                if (jobDifficultyLabel.LabelId === qdlItem.LabelId && jobDifficultyLabel.DifficultyId === qdlItem.DifficultyId) {
                    //alert("found");
                    jobDifficultyLabel.QuestionCount = qdlItem.QuestionCount;
                    return false;
                }
                else {
                    jobDifficultyLabel.QuestionCount = 0;
                }
            });

            if (jobDifficultyLabel.LabelId && jobDifficultyLabel.DifficultyId && jobDifficultyLabel.QuestionCount === 0) {
                this.showToast("No questions found");
            }
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

        private getQuestionDifficultyLabels(): void {
            this.$questionService.getQuestionsByLabelAndDifficulty()
                .then(result => {
                    this.$scope.questionDifficultyLabels = result.data;
                }, error => {
                    this.$log.error('question difficulty labels retrieval failed');
                });
        }

        public removeJobDifficultyLabel(index: number): void {
            this.$scope.jobDifficultyLabelArray.jobDifficultyLabels.splice(index, 1);
        }

        public addJobDifficultyLabel(): void {
            this.$scope.jobDifficultyLabelArray.jobDifficultyLabels.push({});
        }

        private showPrerenderedDialog(): void {
            var dialogOptions: ng.material.IDialogOptions = {
                contentElement: '#uploadStatusModal',
                clickOutsideToClose: false,
                escapeToClose : false,
                scope: this.$scope,
                preserveScope: true
            };

            this.$mdDialog.show(dialogOptions);
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
                    //alert('You clicked the \'UNDO\' action.');
                }
            });
        }

        private createJob(): void {

            this.showPrerenderedDialog();
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
                this.$mdDialog.hide();
                this.$state.go("dashboard");
                this.showToast("Job created");
            }, error => {
                console.log(error);
                this.showToast("Job creation failed");
            });
        }
    }
}