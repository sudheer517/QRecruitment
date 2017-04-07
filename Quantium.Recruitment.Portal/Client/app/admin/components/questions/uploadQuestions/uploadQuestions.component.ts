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
    fileModel;
    fileData: any;
    fileText = "Choose file";
    @ViewChild('questionsPreview') questionsPreviewModal:ModalDirective;
    @ViewChild('progress') progressModal:ModalDirective;
    isRequestProcessing: boolean = true;
    modalResponse: string;
    validationFailed = true;
    questionsSaved = false;

    errorMessage: string;

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
        this.errorMessage = null;
        this.isRequestProcessing = true;
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
                if(error.status == 409){
                    this.validationFailed = true;
                    this.modalResponse = "Duplicate question found for Id:" + error.json();
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                else{
                    this.validationFailed = true;
                    this.isRequestProcessing = false;
                    this.modalResponse = "Questions validation failed";
                    this.errorMessage = error.text();
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
            (error: Response) => {
                if(error.status == 406){
                    this.validationFailed = true;
                    this.modalResponse = "Questions data validation failed. Please upload correct data";
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                if(error.status == 409){
                    this.validationFailed = true;
                    this.modalResponse = "Duplicate question found for Id:" + error.json();
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
                else{
                    this.validationFailed = true;
                    this.isRequestProcessing = false;
                    this.modalResponse = "Questions upload failed";
                }
            }
        );
    }

    browseClick(fileInput: any){
        let fileInputControl: FormControl = fileInput.control as FormControl;
        fileInputControl.reset();
        this.fileText= "Choose file";
    }

    private getFileFormData(eventData: any){
        let formData: FormData = new FormData();

        formData.append('uploadFile', eventData.target.files[0], eventData.target.files[0].name);
        return formData;
    }
}