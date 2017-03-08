import { Component, OnInit, ViewChild } from '@angular/core';
import { JobService } from '../../../services/job.service';
import { CandidateService } from '../../../services/candidate.service';
import { JobDto, CandidateDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { FilterCandidatesPipe } from '../../../pipes/filterCandidates.pipe';
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

    @ViewChild('staticModal') bgModel:ModalDirective;
    selectedtestOptions : SelectedTestOptions;

    selectJob(jobId: number): void { 
        this.selectedJob = this.jobs.find(j => j.Id == jobId);
    }

    constructor(private jobService: JobService, private candidateService: CandidateService){

    }

    ngOnInit(){
        this.selectedtestOptions = new SelectedTestOptions();

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

    generateTests(){
        console.log("test created");
        this.bgModel.show();
    }
    
}