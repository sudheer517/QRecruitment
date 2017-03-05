import { Component, OnInit } from '@angular/core';
import { CandidateService } from '../../../services/candidate.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-view-candidates',
    templateUrl: './viewCandidates.component.html'
})
export class ViewCandidatesComponent implements OnInit{

    candidates: CandidateDto[];
    constructor(private candidateService: CandidateService){

    }

    ngOnInit(){
        this.candidateService.GetAllCandidates().subscribe(
            candidates => {
                this.candidates = candidates;
            },
            error => console.log(error)
        )
    }

}