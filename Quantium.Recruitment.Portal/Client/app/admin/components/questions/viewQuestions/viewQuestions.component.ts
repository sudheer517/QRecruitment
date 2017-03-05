import { Component, OnInit } from '@angular/core';
import { QuestionService } from '../../../services/question.service';
import { QuestionDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-view-questions',
    templateUrl: './viewQuestions.component.html',
    styleUrls: ['./viewQuestions.component.scss']
})
export class ViewQuestionsComponent implements OnInit{
    questions: QuestionDto[];
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
}