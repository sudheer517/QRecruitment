import { Component, OnInit, ViewChild } from '@angular/core';
import { JobService } from '../../../services/job.service';
import { CandidateService } from '../../../services/candidate.service';
import { TestService } from '../../../services/test.service';
import { JobDto, CandidateDto, Candidate_JobDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { FilterCandidatesPipe } from '../../../pipes/filterCandidates.pipe';
import { Observable } from 'rxjs/Observable';
import { Router, ActivatedRoute } from '@angular/router';

class SelectedTestOptions {
    public candidateIds: boolean[];
    constructor() { };
}

@Component({
    selector: 'appc-create-test',
    templateUrl: './createTest.component.html',
})
export class CreateTestComponent implements OnInit{
    @ViewChild('progress') progressModal: ModalDirective;
    isRequestProcessing = true;
    modalResponse: string;

    jobs: JobDto[];
    selectedJobId: number;
    selectedJob: JobDto;

    candidates: CandidateDto[];
    selectedOptionsMap: boolean[];
    hasSelectedAtleastOneCandidate: boolean;
    selectedAll: boolean; 
    filteredCandidates: CandidateDto[];

    @ViewChild('staticModal') bgModel:ModalDirective;
    selectedtestOptions : SelectedTestOptions;

    selectJob(jobId: number): void { 
        this.selectedJob = this.jobs.find(j => j.Id == jobId);
        //console.log(this.hasSelectedAtleastOneCandidate);
    }

    constructor(private jobService: JobService, private candidateService: CandidateService,
    private testService: TestService, private router: Router, private activatedRoute:ActivatedRoute){
    }

    closeProgressModal(){
        this.progressModal.hide();
    }

    ngOnInit(){
        this.selectedtestOptions = new SelectedTestOptions();
        this.selectedtestOptions.candidateIds = [];
        this.selectedOptionsMap = [];
        
        this.jobService.GetAllJobs().subscribe(
            jobs => this.jobs = jobs,
            error => console.log(error)
        );

        this.candidateService.GetCandidateWithoutActiveTests().subscribe(
            candidates => {
                //console.log(candidates);
                this.candidates = candidates;
                this.filteredCandidates = candidates;
            },
            error => console.log(error)
        );
    }
    
    filterCandidates(searchText: string){
        console.log(searchText);
        if(searchText.length > 0){
        this.filteredCandidates = this.candidates.filter(
        candidate => 
                candidate.FirstName.toLocaleLowerCase().indexOf(searchText) != -1 ||
                candidate.LastName.toLocaleLowerCase().indexOf(searchText) != -1 ||
                candidate.Email.toLocaleLowerCase().indexOf(searchText) != -1);
        }
        else{
            this.filteredCandidates = this.candidates;
        }
    }

    updateSelectedCandidateCount(): void {
            var isSelected = false;
            var selectedOptions = this.selectedOptionsMap;
            //console.log(this.selectedtestOptions);
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
        //console.log(this.selectedtestOptions.candidateIds);
        this.progressModal.show();
        let candidateIds = this.selectedtestOptions.candidateIds;
            let candidatesJobs: Candidate_JobDto[] = [];

            candidateIds.forEach((item, index) => {
                if (item === true) {
                    var candidateJob = new Candidate_JobDto();
                    candidateJob.Candidate = new CandidateDto(index);
                    candidateJob.Job = new JobDto(this.selectedJob.Id);
                    candidatesJobs.push(candidateJob);
                }
            });

            this.testService.Generate(candidatesJobs).subscribe(
                result => {
                    console.log(result);
                    this.isRequestProcessing = false;
                    this.modalResponse = "Test generated";
                    this.router.navigate(['viewTests'], { relativeTo: this.activatedRoute});
                },
                error => {
                    this.isRequestProcessing = false;
                    this.modalResponse = "Test generation failed";
                    console.log(error);
                }
            );
            

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