import { Component, OnInit } from '@angular/core';
import { TestService } from '../../../services/test.service';
import { TestDto } from '../../../../RemoteServicesProxy';

@Component({
    selector: 'appc-view-tests',
    templateUrl: './viewTests.component.html'
})
export class ViewTestsComponent implements OnInit{
   tests: TestDto[];
   constructor(private testService: TestService){
   }

   ngOnInit(){
     this.testService.GetAll().subscribe(
       tests => {
         this.tests = tests;
         console.log(tests);
       },
       error => console.log(error)
     )
   }
}