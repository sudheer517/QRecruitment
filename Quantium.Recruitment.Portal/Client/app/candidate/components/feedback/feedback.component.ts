import { Component, OnInit } from '@angular/core';
import { FeedbackService } from '../../services/feedback.service';
import { FeedbackTypeDto, FeedbackDto } from '../../../RemoteServicesProxy';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: '[appc-feedback]',
  templateUrl: './feedback.component.html',
})
export class FeedbackComponent implements OnInit {
  feedbackTypes : FeedbackTypeDto[];
  feedback = new FeedbackDto();
  constructor(private feedbackService: FeedbackService, private router: Router, private activatedRoute: ActivatedRoute,) {
  }

  ngOnInit(){
    
    this.feedbackService.GetAllFeedbackTypes().subscribe(
      feedbackTypes => {
        this.feedbackTypes = feedbackTypes;
        console.log(feedbackTypes);
      }
    );
  }

  saveFeedback(form: FormGroup){
    console.log(this.feedback);
    this.feedbackService.CreateFeedback(this.feedback).subscribe(
       result => {
         console.log(result);
         this.router.navigate(["../testFinished"], { relativeTo : this.activatedRoute});
       }, 
       error => console.log(error)
    );
  }
 }
