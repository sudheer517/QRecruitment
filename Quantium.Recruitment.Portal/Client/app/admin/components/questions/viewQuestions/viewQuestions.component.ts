import { Component, OnInit, ViewChild } from '@angular/core';
import { QuestionService } from '../../../services/question.service';
import { QuestionDto, PagedQuestionDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
    selector: 'appc-view-questions',
    templateUrl: './viewQuestions.component.html',
    styleUrls: ['./viewQuestions.component.scss']
})
export class ViewQuestionsComponent implements OnInit{
    questions: QuestionDto[];
    isRequestProcessing = true;
    modalResponse: string;
    @ViewChild('progress') progressModal: ModalDirective;

    public page:number = 1;
    public length: number = 0;
    public itemsPerPage:number = 10;
    public maxSize:number = 5;
    public numPages:number = 1;

    constructor(private questionService: QuestionService){

    }

    ngOnInit(){
        this.questionService.GetAllQuestionsByPaging(1, this.itemsPerPage).subscribe(
            pagedQuestionDto => {
                console.log(pagedQuestionDto); 
                console.log(pagedQuestionDto.questions); 
                this.questions = pagedQuestionDto.questions;
                this.length = this.questions.length * 2;
                this.numPages = pagedQuestionDto.totalPages;
                this.maxSize = this.numPages;
            },
            error => console.log(error)
        )
    }

    changePage(event: any){
        this.questionService.GetAllQuestionsByPaging(event.page, this.itemsPerPage).subscribe(
            pagedQuestionDto => {
                console.log(pagedQuestionDto); 
                console.log(pagedQuestionDto.questions); 
                this.questions = pagedQuestionDto.questions;
                this.length = this.questions.length * 2;
                this.numPages = pagedQuestionDto.totalPages;
                this.maxSize = this.numPages;
            },
            error => console.log(error)
        )
    }
    
    closeModal(){
        this.progressModal.hide();
    }

    deleteQuestion(questionId: number){
        this.modalResponse = "Deleting question";
        this.progressModal.show();
        console.log(questionId);
        this.questionService.DeleteQuestion(questionId).subscribe(
            deleted => {
                this.isRequestProcessing = false;
                this.modalResponse = "Question deleted";
                this.questions = this.questions.filter(q => q.Id !== questionId);
            },
            error => {
                this.isRequestProcessing = false;
                this.modalResponse = "Unable to delete question";
                console.log("unable to delete question");
            }
        )
    }
}