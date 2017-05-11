import { Component, OnInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'appc-view-survey-questions',
  templateUrl: './view-survey-questions.component.html',
  styleUrls: ['./view-survey-questions.component.scss']
})
export class ViewSurveyQuestionsComponent implements OnInit {

    constructor(private renderer: Renderer, private router: Router, private activatedRoute: ActivatedRoute) {
    }

  ngOnInit() {
  }

}
