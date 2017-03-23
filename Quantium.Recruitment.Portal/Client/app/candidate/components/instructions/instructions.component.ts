import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CandidateService } from '../../services/candidate.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: '[appc-instructions]',
  styleUrls: ['./instructions.component.scss'],
  templateUrl: './instructions.component.html',
})
export class InstructionsComponent {
  constructor(private router: Router, private activatedRoute: ActivatedRoute, private candidateService: CandidateService) {
  }

  ngOnInit(){
    
  }
  
  takeTest(){
    this.router.navigate(["../challenge"], { relativeTo : this.activatedRoute});
  }
 }
