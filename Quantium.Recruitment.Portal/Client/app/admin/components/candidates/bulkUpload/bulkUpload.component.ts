import { Component, Renderer, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CandidateService } from '../../../services/candidate.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Response } from '@angular/http';

@Component({
    selector: 'appc-bulk-upload',
    templateUrl: './bulkUpload.component.html',
    styleUrls: ['./bulkUpload.component.scss'],
})
export class BulkUploadComponent implements OnInit {
    modalResponse: string;
    fileModel;
    isRequestProcessing = true;
    candidates: CandidateDto[];
    fileData: any;
    fileText = "Choose file";
    @ViewChild('progress') progressModal: ModalDirective;
    @ViewChild('previewCandidatesModal') previewModal: ModalDirective;
    validationFailed = true;
    candidatesSaved = false;

    constructor(private renderer: Renderer, private candidateService: CandidateService, private router: Router,
    private activatedRoute:ActivatedRoute){
        
    }
    
    closeProgressModal(){
        this.progressModal.hide();
        if(!this.validationFailed && this.candidatesSaved){
            this.router.navigate(['../viewCandidates'], { relativeTo: this.activatedRoute});
        }
    }

    ngOnInit(){
        let body = document.getElementsByTagName('input')[0];
        this.renderer.setElementAttribute(body, "accept", ".xlsx");
    }

    previewCandidates(modalContent: FormControl){
        this.previewModal.show();
    }

    onFileChange(eventData: any){
        this.fileText = eventData.target.value.split("\\").pop();
        this.modalResponse = "Validating candidates";
        this.progressModal.show();
        this.fileData = eventData;
        let formData = this.getFileFormData(this.fileData); 
        this.candidateService.PreviewCandidates(formData).subscribe(
            candidates => {
                this.candidates = candidates;
                this.modalResponse = "Validation successful";
                this.validationFailed = false;
                this.isRequestProcessing = false;
            },
            (error: Response) =>{
                if(error.status == 406){
                    this.validationFailed = true;
                    this.modalResponse = "Candidates validation failed. Please upload correct data";
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                if(error.status == 422){
                    this.validationFailed = true;
                    this.modalResponse = "No candidates found. Check your data";
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                if(error.status == 409){
                    this.validationFailed = false;
                    //this.modalResponse = "Duplicate candidate found " + (error.json() as CandidateDto).Email;
                    this.modalResponse = "Duplicate candidate found. Check your data";
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                console.log(error)
            } 
        );       
    }

    browseClick(fileInput: any){
        let fileInputControl: FormControl = fileInput.control as FormControl;
        fileInputControl.reset();
        this.fileText= "Choose file";
    }

    addCandidates(modalContent: FormControl){
        this.progressModal.show();
        this.modalResponse = "Adding candidates";

        let formData = this.getFileFormData(this.fileData); 
        this.candidateService.AddCandidates(formData).subscribe(
            status => {
                this.isRequestProcessing = false;
                this.modalResponse = "Candidates added successfully";
                this.candidatesSaved = true;
            }, 
            error => {
                if (error.status == 409) {
                    this.modalResponse = "Skipped duplicate candidates";
                }
                else {
                    this.modalResponse = "Unable to add candidates";
                }
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