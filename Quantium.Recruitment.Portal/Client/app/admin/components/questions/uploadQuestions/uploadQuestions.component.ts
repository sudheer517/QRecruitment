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
})
export class UploadQuestionsComponent implements OnInit{
    questions: QuestionDto[];
    fileData: any;
    fileText = "Choose file";
    @ViewChild('questionsPreview') questionsPreviewModal:ModalDirective;
    @ViewChild('progress') progressModal:ModalDirective;
    isRequestProcessing: boolean = true;
    modalResponse: string;

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
        this.router.navigate(['viewQuestions'], { relativeTo: this.activatedRoute});
        
    }

    previewQuestions(modalContent: FormControl){
        this.open(modalContent);
    }

    onFileChange(eventData: any){
        this.fileText = eventData.target.value.split("\\").pop();

        this.fileData = eventData;
        let formData = this.getFileFormData(this.fileData); 
        this.questionService.PreviewQuestions(formData).subscribe(
            questions => this.questions = questions, error => console.log(error)
        );       
    }

    addQuestions(modalContent: FormControl){
        this.progressModal.show();
        this.modalResponse = "Uploading question";
        let formData = this.getFileFormData(this.fileData); 
        this.questionService.AddQuestions(formData).subscribe(
            status => {
                this.isRequestProcessing = false;
                this.modalResponse = "Questions uploaded";
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