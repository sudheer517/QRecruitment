import { Component, OnInit, ViewEncapsulation, ElementRef, Renderer } from '@angular/core';

@Component({
    selector: "[appc-questions]",
    templateUrl: "./questions.component.html",
    styleUrls: ["./questions.component.scss"],
    // encapsulation: ViewEncapsulation.None
})
export class QuestionsComponent implements OnInit{
    locationChartOptions: Object;
    branchChartOptions: Object;
    teamChartOptions: Object;
    constructor(private el: ElementRef, private renderer: Renderer) {
    }

    ngOnInit(){
       this.changeBackground();
    }

    changeBackground(){
        let body = document.getElementsByTagName('body')[0];
        body.className.split(' ').forEach(item => item.trim().length > 0 && body.classList.remove(item));
        body.classList.add("questions-tests-background");
    }

}