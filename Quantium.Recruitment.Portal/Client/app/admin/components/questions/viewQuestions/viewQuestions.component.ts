import { Component, OnInit, ViewChild } from '@angular/core';
import { QuestionService } from '../../../services/question.service';
import { QuestionDto } from '../../../../RemoteServicesProxy';
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

    constructor(private questionService: QuestionService){

    }

    ngOnInit(){
        this.questionService.GetAllQuestions().subscribe(
            questions => {
                console.log(questions); 
                this.questions = questions
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