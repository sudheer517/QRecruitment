import { Component, OnInit, ViewChild } from '@angular/core';
import { JobService } from '../../../services/job.service';
import { CandidateService } from '../../../services/candidate.service';
import { JobDto, CandidateDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { FilterCandidatesPipe } from '../../../pipes/filterCandidates.pipe';
import { Observable } from 'rxjs/Observable';

class SelectedTestOptions {
    public candidateIds: boolean[];
    constructor() { };
}

@Component({
    selector: 'appc-create-test',
    templateUrl: './createTest.component.html'
})
export class CreateTestComponent implements OnInit{
    jobs: JobDto[];
    selectedJobId: number;
    selectedJob: JobDto;
    candidates: CandidateDto[];
    selectedOptionsMap: any;
    hasSelectedAtleastOneCandidate: boolean;
    selectedAll: boolean; 

    @ViewChild('staticModal') bgModel:ModalDirective;
    selectedtestOptions : SelectedTestOptions;

    selectJob(jobId: number): void { 
        this.selectedJob = this.jobs.find(j => j.Id == jobId);
    }

    constructor(private jobService: JobService, private candidateService: CandidateService){

    }

    ngOnInit(){
        this.selectedtestOptions = new SelectedTestOptions();
        this.selectedtestOptions.candidateIds = [];
        this.selectedOptionsMap = {};
        Observable.of(this.selectedtestOptions.candidateIds).subscribe(
            values => this.updateSelectedCandidateCount(), error => console.log(error)
        )
        //this.selectedtestOptions.candidateIds
        this.jobService.GetAllJobs().subscribe(
            jobs => this.jobs = jobs,
            error => console.log(error)
        );

        this.candidateService.GetCandidateWithoutActiveTests().subscribe(
            candidates => {
                console.log(candidates);
                this.candidates = candidates;
            },
            error => console.log(error)
        );
    }
    
    updateSelectedCandidateCount(): void {
            
            var isSelected = false;
            var selectedOptions = this.selectedOptionsMap;
            if(this.selectedtestOptions.candidateIds){
                this.selectedtestOptions.candidateIds.forEach((item, index) => {
                    if (item === true) {
                        isSelected = true;
                        selectedOptions[index] = isSelected;
                    }
                });
            }

            this.hasSelectedAtleastOneCandidate = isSelected;
            this.selectedOptionsMap = selectedOptions;
        }

    generateTests(){
        console.log("test created");
        this.bgModel.show();
    }

    checkAll(filteredCandidates: CandidateDto[]): void {

            let isSelected;

            if (this.selectedAll) {
                isSelected = true;
            } else {
                isSelected = false;
            }

            var selectedOptions = this.selectedOptionsMap;

            if(filteredCandidates){
            filteredCandidates.forEach((filteredCandidate, index) => {  
                selectedOptions[filteredCandidate.Id] = isSelected;
            });
            }

            this.selectedtestOptions.candidateIds = selectedOptions;
        }
    
}