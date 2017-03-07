import { Component, OnInit, ViewChild } from '@angular/core';
import { JobService } from '../../../services/job.service';
import { JobDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';

@Component({
    selector: 'appc-create-test',
    templateUrl: './createTest.component.html'
})
export class CreateTestComponent implements OnInit{
    jobs: JobDto[];
    selectedJobId: number;
    selectedJob: JobDto;
    @ViewChild('staticModal') bgModel:ModalDirective;
    
    selectJob(jobId: number): void { 
        this.selectedJob = this.jobs.find(j => j.Id == jobId);
    }

    constructor(private jobService: JobService){

    }

    ngOnInit(){
        this.jobService.GetAllJobs().subscribe(
            jobs => this.jobs = jobs,
            error => console.log(error)
        )
    }

    generateTests(){
        console.log("test created");
        this.bgModel.show();
    }
    
}