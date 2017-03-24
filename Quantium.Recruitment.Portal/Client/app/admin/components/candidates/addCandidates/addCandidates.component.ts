import { Component, ViewChild } from '@angular/core';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { CandidateService } from '../../../services/candidate.service';
import { FormGroup } from '@angular/forms';
import { Response } from '@angular/http';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
    selector: 'appc-add-candidates',
    templateUrl: './addCandidates.component.html'
})
export class AddCandidatesComponent{
    @ViewChild('progress') progressModal:ModalDirective;
    isRequestProcessing: boolean = true;
    modalResponse: string;

    candidate = new CandidateDto();
    constructor(private candidateService: CandidateService){

    }

    addCandidate(form: FormGroup){
        this.modalResponse = "Adding candidate";
        this.isRequestProcessing = true;
        this.progressModal.show();
        this.candidateService.AddCandidate(this.candidate).subscribe(
            candidate => {
                this.isRequestProcessing = false;
                this.modalResponse = "Candidate added";
                console.log(candidate);
                form.reset();
            },
            (error: Response) => {
                this.isRequestProcessing = false;
                this.modalResponse = "Unable to add candidate";
                if(error.status === 409){
                    this.modalResponse = "Duplicate candidate found";
                }
            }
        );

    }
}