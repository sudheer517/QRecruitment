import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CandidateDto } from '../../../RemoteServicesProxy';
import { CandidateService } from '../../services/candidate.service';
import { FormControl, FormGroup, FormBuilder, Validators, AbstractControl} from '@angular/forms';

@Component({
  selector: '[appc-candidate-details]',
  styleUrls: ['./candidateDetails.component.scss'],
  templateUrl: './candidateDetails.component.html',
})
export class CandidateDetailsComponent implements OnInit{
  candidate: CandidateDto;
  candidateDetailsForm: FormGroup;
  years = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15];
  constructor(
    private router: Router, 
    private activatedRoute: ActivatedRoute, 
    private cadidateService: CandidateService,
    private formBuilder: FormBuilder,) {
  }
  
  
  ngOnInit(){
    this.candidate = new CandidateDto();

    this.candidateDetailsForm = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            mobile : ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10), this.mobileValidator]],
            branch: ['', Validators.required],
            cgpa: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(3), this.cgpaValidator]],
            college: ['', Validators.required],
            state: ['', Validators.required],
            city: ['', Validators.required],
            country: ['', Validators.required],
            graduation: ['', [Validators.required,  this.graduationValidator]],
            currentCompany: ['', Validators.required],
            experience: ['', Validators.required],
        });
  }

  mobileValidator(c: AbstractControl): {[key: string]: boolean} | null {
      if(c.value != undefined && !(!isNaN(parseFloat(c.value)) && isFinite(c.value))){
            return { 'notnumeric': true};
      }
      return null;
  }

  graduationValidator(c: AbstractControl): {[key: string]: boolean} | null {
      if(c.value != undefined){
          if(!(!isNaN(parseFloat(c.value)) && isFinite(c.value))){
                return { 'notnumeric': true};
          }
          let number = +c.value;
          if(number< 1900 || number > 2025){
                return { 'yearNotValid': true};
          }
      }
      return null;
  }

  cgpaValidator(c: AbstractControl): {[key: string]: boolean} | null {
      if(c.value != undefined){
          if(!(!isNaN(parseFloat(c.value)) && isFinite(c.value))){
                let number = +c.value;
                return { 'notnumeric': true};
          }
          let number = +c.value;
          if(number > 10){
            return { 'morethan10': true};
          }
      }
      return null;
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
