import { Component, OnInit, ViewChild } from '@angular/core';
import { CandidateService } from '../../../services/candidate.service';
import { SurveyService } from '../../../services/survey.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { FilterCandidatesPipe } from '../../../pipes/filterCandidates.pipe';
import { Observable } from 'rxjs/Observable';
import { Router, ActivatedRoute } from '@angular/router';

class SelectedSurveyOptions {
    public candidateIds: boolean[];
    constructor() { };
}
@Component({
    selector: 'appc-create-survey',
    templateUrl: './createSurvey.component.html'
})
export class CreateSurveyComponent implements OnInit {
    @ViewChild('progress') progressModal: ModalDirective;

    candidates: CandidateDto[];
    hasSelectedAtleastOneCandidate: boolean;
    selectedOptionsMap: boolean[];
    selectedAll: boolean;
    filteredCandidates: CandidateDto[];

    @ViewChild('staticModal') bgModel: ModalDirective;
    selectedSurveyOptions: SelectedSurveyOptions;

constructor(private candidateService: CandidateService, private surveyService: SurveyService, 
private router: Router, private activatedRoute:ActivatedRoute){
}

ngOnInit() {
        this.selectedSurveyOptions = new SelectedSurveyOptions();
        this.selectedSurveyOptions.candidateIds = [];
        this.selectedOptionsMap = [];

        this.candidateService.GetCandidateWithoutActiveTests().subscribe(
            candidates => {
                this.candidates = candidates;
                this.filteredCandidates = candidates;
            },
            error => console.log(error)
    );
}

filterCandidates(searchText: string){
    console.log(searchText);
    if (searchText.length > 0) {
        this.filteredCandidates = this.candidates.filter(
            candidate =>
                candidate.FirstName.toLocaleLowerCase().indexOf(searchText) != -1 ||
                candidate.LastName.toLocaleLowerCase().indexOf(searchText) != -1 ||
                candidate.Email.toLocaleLowerCase().indexOf(searchText) != -1);
    }
    else {
        this.filteredCandidates = this.candidates;
    }
}
updateSelectedCandidateCount(): void {
    var isSelected = false;
    var selectedOptions = this.selectedOptionsMap;

    if (this.selectedSurveyOptions.candidateIds) {
        this.selectedSurveyOptions.candidateIds.forEach((item, index) => {
            if (item === true) {
                isSelected = true;
                selectedOptions[index] = isSelected;
            }
        });
    }

    this.hasSelectedAtleastOneCandidate = isSelected;
    this.selectedOptionsMap = selectedOptions;
}

generateSurveys(){

    this.progressModal.show();
    let candidateIds = this.selectedSurveyOptions.candidateIds;
    let candidates: CandidateDto[] = [];

    this.surveyService.Generate(candidates).subscribe(
        result => {
            console.log(result);
            this.router.navigate(['viewSurveys'], {relativeTo: this.activatedRoute});
        },
        error => console.log(error)

    // this.bgModel.show();
    )
}

checkAll(filteredCandidates: CandidateDto[]): void {

    let isSelected;

    if(this.selectedAll) {
        isSelected = true;
    } else {
        isSelected = false;
    }

    var selectedOptions = this.selectedOptionsMap;

    if(filteredCandidates) {
        filteredCandidates.forEach((filteredCandidate, index) => {
            selectedOptions[filteredCandidate.Id] = isSelected;
        });
    }

    this.selectedSurveyOptions.candidateIds = selectedOptions;
}
    
}