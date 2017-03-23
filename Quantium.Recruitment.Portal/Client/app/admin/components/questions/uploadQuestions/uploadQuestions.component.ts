import { Component, Renderer, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { QuestionService } from '../../../services/question.service';
import { QuestionDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Response } from '@angular/http';

@Component({
    selector: 'appc-upload-questions',
    templateUrl: './uploadQuestions.component.html',
    styleUrls: ['./uploadQuestions.component.scss'],
})
export class UploadQuestionsComponent implements OnInit{
    questions: QuestionDto[];
    fileData: any;
    fileText = "Choose file";
    @ViewChild('questionsPreview') questionsPreviewModal:ModalDirective;
    @ViewChild('progress') progressModal:ModalDirective;
    isRequestProcessing: boolean = true;
    modalResponse: string;
    validationFailed = true;
    questionsSaved = false;

    constructor(private renderer: Renderer, private questionService: QuestionService,  private router: Router, private activatedRoute:ActivatedRoute){
    }
    open(content) {
        this.questionsPreviewModal.show();
    }
    ngOnInit(){
        let body = document.getElementsByTagName('input')[0];
    }

    closeProgressModal(){
        this.progressModal.hide();
        if(!this.validationFailed && this.questionsSaved){
            this.router.navigate(['viewQuestions'], { relativeTo: this.activatedRoute});
        }
        
    }

    previewQuestions(modalContent: FormControl){
        this.open(modalContent);
    }

    onFileChange(eventData: any){
        this.fileText = eventData.target.value.split("\\").pop();
        this.modalResponse = "Validating questions";
        this.progressModal.show();
        this.fileData = eventData;
        let formData = this.getFileFormData(this.fileData); 
        this.questionService.PreviewQuestions(formData).subscribe(
            questions => {
                this.questions = questions;
                this.modalResponse = "Validation successful";
                this.isRequestProcessing = false;
                this.validationFailed = false;
            },
            (error: Response) => {
                if(error.status == 406){
                    this.validationFailed = true;
                    this.modalResponse = "Questions data validation failed. Please upload correct data";
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                console.log(error);
            }
        );       
    }

    addQuestions(modalContent: FormControl){
        this.isRequestProcessing = true;
        this.progressModal.show();
        this.modalResponse = "Uploading questions";
        let formData = this.getFileFormData(this.fileData); 
        this.questionService.AddQuestions(formData).subscribe(
            status => {
                this.isRequestProcessing = false;
                this.modalResponse = "Questions uploaded";
                this.questionsSaved = true;
            }, 
            error => {
                console.log(error);
                this.isRequestProcessing = false;
                this.modalResponse = "Questions upload failed";
            }
        );
    }

    private getFileFormData(eventData: any){
        let formData: FormData = new FormData();

        formData.append('uploadFile', eventData.target.files[0], eventData.target.files[0].name);
        return formData;
    }
}