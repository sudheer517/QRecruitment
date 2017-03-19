import { Component, Renderer, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CandidateService } from '../../../services/candidate.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
    selector: 'appc-bulk-upload',
    templateUrl: './bulkUpload.component.html',
    styleUrls: ['./bulkUpload.component.scss'],
})
export class BulkUploadComponent implements OnInit {
    modalResponse: string;
    isRequestProcessing = true;
    candidates: CandidateDto[];
    fileData: any;
    fileText = "Choose file";
    @ViewChild('progress') progressModal: ModalDirective;
    @ViewChild('previewCandidatesModal') previewModal: ModalDirective;

    constructor(private renderer: Renderer, private candidateService: CandidateService, private router: Router,
    private activatedRoute:ActivatedRoute){
        
    }
    
    closeProgressModal(){
        this.progressModal.hide();
        this.router.navigate(['../viewCandidates'], { relativeTo: this.activatedRoute});
    }

    ngOnInit(){
        let body = document.getElementsByTagName('input')[0];
        this.renderer.setElementAttribute(body, "accept", ".xlsx");
    }

    previewCandidates(modalContent: FormControl){
        this.previewModal.show();
    }

    onFileChange(eventData: any){
        console.log("file change");
        console.log(event);
        this.fileText = eventData.target.value.split("\\").pop();
        this.fileData = eventData;
        let formData = this.getFileFormData(this.fileData); 
        this.candidateService.PreviewCandidates(formData).subscribe(
            candidates => this.candidates = candidates, error => console.log(error)
        );       
    }

    addCandidates(modalContent: FormControl){
        this.progressModal.show();
        this.modalResponse = "Adding candidates";

        let formData = this.getFileFormData(this.fileData); 
        this.candidateService.AddCandidates(formData).subscribe(
            status => {
                this.isRequestProcessing = false;
                this.modalResponse = "Candidates added successfully";
                console.log(status);
                //
            }, 
            error => {
                this.modalResponse = "Unable to add candidates";
                console.log(error);
                this.isRequestProcessing = false;
            }
        );
    }

    private getFileFormData(eventData: any){
        let formData: FormData = new FormData();
        formData.append('uploadFile', eventData.target.files[0], eventData.target.files[0].name);
        return formData;
    }
}