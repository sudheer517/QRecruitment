import { Component, OnInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SurveyQuestionDto } from '../../../../RemoteServicesProxy';
import { SurveyService } from '../../../services/survey.service';


@Component({
  selector: 'appc-view-survey-questions',
  templateUrl: './view-survey-questions.component.html',
  styleUrls: ['./view-survey-questions.component.scss']
})
export class ViewSurveyQuestionsComponent implements OnInit {
    questions: SurveyQuestionDto[];

    constructor(private questionService: SurveyService,private renderer: Renderer, private router: Router, private activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
        this.getQuestions();
      
    }
    getQuestions() {
        this.questionService.GetAllQuestions().subscribe(
            a => {
                this.questions = a;
            },
            error => console.log(error)
        )
    }
    deleteQuestion(questionId : number)
    {
        this.questionService.DeleteQuestion(questionId).subscribe(
            deleted => {
                this.questions = this.questions.filter(q => q.Id !== questionId);
            },
            error => console.log(error)
        )
    }

}
