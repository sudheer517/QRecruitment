
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
        isQuestionsFound: boolean;
        toggleSidenav(): void;
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
            private $questionService: Services.QuestionService,
            private $timeout: ng.ITimeoutService,
            private $mdSidenav: ng.material.ISidenavService) {

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
                this.$scope.toggleSidenav = () => this.toggleSidenav();
                this.$scope.showAvailableQuestions = (jobDifficultyLabel: any) => this.showAvailableQuestions(jobDifficultyLabel);
        }

        private toggleSidenav(): void {
            this.$mdSidenav("left").toggle();
        }

        private showAvailableQuestions(jobDifficultyLabel: any): void {
            jobDifficultyLabel.UserQuestionCount = null;
            _.each(this.$scope.questionDifficultyLabels, (qdlItem, qdlIndex) => {
                if (jobDifficultyLabel.LabelId === qdlItem.LabelId && jobDifficultyLabel.DifficultyId === qdlItem.DifficultyId) {
                    jobDifficultyLabel.QuestionCount = qdlItem.QuestionCount;
                    return false;
                }
                else {
                    jobDifficultyLabel.QuestionCount = 0;
                    
                }
            });

            if ((jobDifficultyLabel.LabelId && jobDifficultyLabel.DifficultyId && jobDifficultyLabel.QuestionCount === 0) ||
                (jobDifficultyLabel.LabelId && jobDifficultyLabel.DifficultyId && (!jobDifficultyLabel.QuestionCount))) {
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
                    this.showToast("No questions found.Please add questions");
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
            if ((this.$scope.jobDifficultyLabelArray.jobDifficultyLabels.length === 0)) {
                this.showToast("Please fill label and difficulty");
                return;
            }
            
            this.showPrerenderedDialog();
            var job = this.$scope.job;
            var labelIds = [];
            var difficultyIds = [];
            var displayQuestionCounts = [];
            var passingQuestionCounts = [];
            _.each(this.$scope.jobDifficultyLabelArray.jobDifficultyLabels, (item, index) => {
                labelIds.push(item.LabelId);
                difficultyIds.push(item.DifficultyId);
                displayQuestionCounts.push(item.UserQuestionCount);
                passingQuestionCounts.push(item.PassingQuestionCount);
            });

            job.JobDifficultyLabels = [];

            _.each(labelIds, (item, index) => {
                var jobDifficultyLabel = new JobDifficultyLabelDto();
                jobDifficultyLabel.Label = new LabelDto(labelIds[index]);
                jobDifficultyLabel.Difficulty = new DifficultyDto(difficultyIds[index]);
                jobDifficultyLabel.DisplayQuestionCount = displayQuestionCounts[index];
                jobDifficultyLabel.PassingQuestionCount = passingQuestionCounts[index];
                job.JobDifficultyLabels.push(jobDifficultyLabel);
            });

            this.$http.post("/Job/Create", job).then(response => {
                console.log(response);
                this.$mdDialog.hide();
                this.$timeout(() => {
                    this.$state.go("dashboard");
                }, 1000);
                this.showToast("Job created");
            }, error => {
                console.log(error);
                this.$mdDialog.hide();
                this.showToast("Job creation failed");
            });
        }
    }
}