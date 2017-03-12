import { Component, OnInit, ViewEncapsulation, ElementRef, Renderer} from '@angular/core';

@Component({
    selector: "[appc-jobs]",
    templateUrl: "./jobs.component.html",
    styleUrls: ["./jobs.component.scss"],
})
export class JobsComponent implements OnInit{
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
        body.classList.add("jobs-addAdmin-background");
    }
   
}