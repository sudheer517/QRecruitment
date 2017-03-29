import { Component } from '@angular/core';
import { JobDto } from '../../../../RemoteServicesProxy';
import { JobService } from '../../../services/job.service';
import { TestService } from '../../../services/test.service';

@Component({
    selector: 'appc-view-jobs',
    templateUrl: './viewJobs.component.html',
    styleUrls: ['./viewJobs.component.scss']
})
export class ViewJobsComponent{
    jobs: JobDto[];
    constructor(private jobService: JobService, private testService: TestService){

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

    exportFinishedTests(job: JobDto, jobIndex: number){
        
        this.testService.GetExcelFileForFinishedTestsByJob(job.Id).subscribe(
            data => {
                var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

                if (navigator.msSaveOrOpenBlob) { // for IE and Edge
                    navigator.msSaveOrOpenBlob( blob, `Finished tests - ${job.Title}.xlsx` );
                }
                else {
                    var link=document.createElement('a');
                    document.body.appendChild(link);
                    link.href=window.URL.createObjectURL(blob);
                    link.download=`Finished tests - ${job.Title}.xlsx`;
                    link.click();
                }
            },
            error => console.log(error)
        )
    }

    deleteJob(job: JobDto, jobIndex: number){
        
        this.jobService.DeleteJob(job.Id).subscribe(
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