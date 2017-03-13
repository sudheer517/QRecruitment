import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CandidateDto } from '../../../RemoteServicesProxy';
import { CandidateService } from '../../services/candidate.service';

@Component({
  selector: '[appc-candidate-details]',
  styleUrls: ['./candidateDetails.component.scss'],
  templateUrl: './candidateDetails.component.html',
})
export class CandidateDetailsComponent implements OnInit{
  constructor(private router: Router, private activatedRoute: ActivatedRoute, private cadidateService: CandidateService) {
  }
  
  candidate: CandidateDto;

  ngOnInit(){
    this.candidate = new CandidateDto();
  }

  saveCandidateDetails(form: any){
      this.cadidateService.SaveDetails(this.candidate).subscribe(
        result => {
            console.log("saveCandidate response :" + result);
            this.router.navigate(["../instructions"], { relativeTo : this.activatedRoute});
        },
        error => console.log(error)
      );
       
  }
 }
