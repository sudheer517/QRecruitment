
module Recruitment.Controllers {
    import JobDto = Quantium.Recruitment.ODataEntities.JobDto;
    import DepartmentDto = Quantium.Recruitment.ODataEntities.DepartmentDto;
    import LabelDto = Quantium.Recruitment.ODataEntities.LabelDto;
    import DifficultyDto = Quantium.Recruitment.ODataEntities.DifficultyDto;

    interface ICreateJobControllerScope extends ng.IScope {
        job: JobDto;
        departments: DepartmentDto[];
        labels: LabelDto[];
        difficulties: DifficultyDto[];
        selectedDepartment: DepartmentDto;
        toggleLabelSelection: (labelId: number) => void;
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

        private toggleLabelSelection(labelId: number): void {

        }
    }
}