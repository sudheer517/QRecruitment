import { Component, Renderer, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { QuestionService } from '../../../services/question.service';
import { QuestionDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
    selector: 'appc-upload-questions',
    templateUrl: './uploadQuestions.component.html',
    styleUrls: ['./uploadQuestions.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class UploadQuestionsComponent implements OnInit{
    smallModalStatus = false;
    questions: QuestionDto[];
    fileData: any;
    @ViewChild('questionsPreview') questionsPreviewModal:ModalDirective;
    @ViewChild('progress') progressModal:ModalDirective;

    constructor(private renderer: Renderer, private questionService: QuestionService,  private router: Router,
    private activatedRoute:ActivatedRoute){
        
    }
    open(content) {
        this.questionsPreviewModal.show();
        //this.modalRef = this.modalService.open(content, { windowClass: "large-modal-window" });

    }
    ngOnInit(){
        let body = document.getElementsByTagName('input')[0];
        this.renderer.setElementAttribute(body, "accept", ".xlsx");
    }

    previewQuestions(modalContent: FormControl){
        this.open(modalContent);
    }

    onFileChange(eventData: any){
        this.fileData = eventData;
        let formData = this.getFileFormData(this.fileData); 
        this.questionService.PreviewQuestions(formData).subscribe(
            questions => this.questions = questions, error => console.log(error)
        );       
    }

    addQuestions(modalContent: FormControl){
        this.progressModal.show();
        //this.smallModalRef = this.modalService.open(modalContent, { keyboard: false, backdrop: "static", windowClass: "modal-window" });
        let formData = this.getFileFormData(this.fileData); 
        this.questionService.AddQuestions(formData).subscribe(
            status => {
                console.log(status);
                //this.smallModalRef.close();
                this.router.navigate(['viewQuestions'], { relativeTo: this.activatedRoute});
            }, 
            error => {
                console.log(error);
                this.smallModalStatus = true;
            }
        );
    }

    private getFileFormData(eventData: any){
        let formData: FormData = new FormData();
        formData.append('uploadFile', eventData.file, eventData.file.name);
        return formData;
    }
}