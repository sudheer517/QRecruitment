
module Recruitment.Controllers {

    import CandidateDto = Quantium.Recruitment.ODataEntities.CandidateDto;

    interface ICandidatesListControllerScope extends ng.IScope {
        candidates: CandidateDto[];
    }

    export class CandidatesListController {
        constructor(
            private $scope: ICandidatesListControllerScope,
            private $candidateService: Recruitment.Services.CandidateService) {
            this.getAllCandidates();
        }

        private getAllCandidates(): void{
            this.$candidateService.getAllCandidates().then(
                response => {
                    this.$scope.candidates = response.data;
                },
                error => {
                    console.log(error);

                });
        }
    }
}