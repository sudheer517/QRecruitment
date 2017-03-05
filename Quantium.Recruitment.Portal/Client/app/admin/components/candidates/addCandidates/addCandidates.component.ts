import { Component } from '@angular/core';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { CandidateService } from '../../../services/candidate.service';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'appc-add-candidates',
    templateUrl: './addCandidates.component.html'
})
export class AddCandidatesComponent{
    
    candidate = new CandidateDto();
    constructor(private candidateService: CandidateService){

    }

    addCandidate(form: FormGroup){
        this.candidateService.AddCandidate(this.candidate).subscribe(
            candidate => {
                console.log(candidate);
                form.reset();
            },
            error => console.log(error)
        );

    }
}