import { Component } from '@angular/core';
import { JobDto } from '../../../../RemoteServicesProxy';
import { JobService } from '../../../services/job.service';

@Component({
    selector: 'appc-view-jobs',
    templateUrl: './viewJobs.component.html',
    styleUrls: ['./viewJobs.component.scss']
})
export class ViewJobsComponent{
    jobs: JobDto[];
    constructor(private jobService: JobService){

    }

    ngOnInit(){
        this.jobService.GetAllJobs().subscribe(
            jobs => {
                this.jobs = jobs;
            },
            error => console.log(error)
        )
    }

    getDisplayQuestionCount(currentJob: JobDto) {
        var total = 0;
        for (var i = 0; i < currentJob.JobDifficultyLabels.length; i++) {
            total += currentJob.JobDifficultyLabels[i].DisplayQuestionCount;
        }
        return total;
    }

    getPassingQuestionCount(currentJob: JobDto) {
        var total = 0;
        for (var i = 0; i < currentJob.JobDifficultyLabels.length; i++) {
            total += currentJob.JobDifficultyLabels[i].PassingQuestionCount;
        }
        return total;
    }

    deleteJob(job: JobDto, jobIndex: number){
        this.jobService.DeleteJob(job).subscribe(
            result => {
                console.log(this.jobs);
                console.log("deleting");//let deleteIndex = this.jobs.findIndex(item => item === job);
                this.jobs.splice(jobIndex, 1);
                console.log(this.jobs);
            },
            error => console.log(error)
        )
    }

}