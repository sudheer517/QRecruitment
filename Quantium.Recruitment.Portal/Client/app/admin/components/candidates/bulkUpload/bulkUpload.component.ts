import { Component, Renderer, ViewEncapsulation, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CandidateService } from '../../../services/candidate.service';
import { CandidateDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'appc-bulk-upload',
    templateUrl: './bulkUpload.component.html',
    styleUrls: ['./bulkUpload.component.scss'],
    //encapsulation: ViewEncapsulation.None
})
export class BulkUploadComponent implements OnInit {

    smallModalStatus = false;
    candidates: CandidateDto[];
    fileData: any;
    fileText = "Choose file";
    constructor(private renderer: Renderer, private candidateService: CandidateService, private router: Router,
    private activatedRoute:ActivatedRoute){
        
    }
    open(content) {
        //this.modalRef = this.modalService.open(content, { windowClass: "large-modal-window" });

    }
    ngOnInit(){
        let body = document.getElementsByTagName('input')[0];
        this.renderer.setElementAttribute(body, "accept", ".xlsx");
    }

    previewCandidates(modalContent: FormControl){
        this.open(modalContent);
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
        //this.smallModalRef = this.modalService.open(modalContent, { keyboard: false, backdrop: "static", windowClass: "modal-window" });
        let formData = this.getFileFormData(this.fileData); 
        this.candidateService.AddCandidates(formData).subscribe(
            status => {
                console.log(status);
                //this.smallModalRef.close();
                this.router.navigate(['../viewCandidates'], { relativeTo: this.activatedRoute});
            }, 
            error => {
                console.log(error);
                this.smallModalStatus = true;
            }
        );
    }

    private getFileFormData(eventData: any){
        let formData: FormData = new FormData();
        formData.append('uploadFile', eventData.target.files[0], eventData.target.files[0].name);
        return formData;
    }
}