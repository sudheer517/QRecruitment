import { Component, OnInit, ViewEncapsulation, ElementRef, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'appc-survey',
  templateUrl: './survey.component.html',
  styleUrls: ['./survey.component.scss']
})
export class SurveyComponent implements OnInit {

    locationChartOptions: Object;
    branchChartOptions: Object;
    teamChartOptions: Object;
    constructor(private el: ElementRef, private renderer: Renderer, private router: Router, private activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
        // this.changeBackground();
        this.router.navigate(["surveyQuestions"], { relativeTo: this.activatedRoute });        
    }

    changeBackground() {
        let body = document.getElementsByTagName('body')[0];
        body.className.split(' ').forEach(item => item.trim().length > 0 && body.classList.remove(item));
        body.classList.add("questions-tests-background");
    }
}
