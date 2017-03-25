import { Component, OnInit,ChangeDetectorRef, ViewChild  } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, AbstractControl, FormArray, ValidatorFn } from '@angular/forms';

import { JobService } from '../../../services/job.service';
import { DepartmentService } from '../../../services/department.service';
import { LabelService } from '../../../services/label.service';
import { DifficultyService } from '../../../services/difficulty.service';
import { QuestionService } from '../../../services/question.service';

import { JobDto, DepartmentDto, LabelDto, DifficultyDto, Question_Difficulty_LabelDto, Job_Difficulty_LabelDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Router, ActivatedRoute } from '@angular/router';




@Component({
    selector: 'appc-create-job',
    templateUrl: './createJob.component.html',
    styleUrls: ['./createJob.component.scss']
})
export class CreateJobComponent implements OnInit {
    jobForm: FormGroup;
    job: JobDto = new JobDto();
    availableQuestionsMap = [];
    questionsToPassMap = [];
    @ViewChild('progress') progressModal:ModalDirective;
    isRequestProcessing: boolean = true;
    modalResponse: string;
    isEnteredTitleExists: boolean = false;
    selectedLabelAndDiffs = [];

    get labelsAndDifficulties(): FormArray { 
        return this.jobForm.get('labelsAndDifficulties') as FormArray; 
    }

    departments: DepartmentDto[];
    labels: LabelDto[];
    difficulties: DifficultyDto[];
    questionDifficultyLabels: Question_Difficulty_LabelDto[];


    constructor(
        private jobService: JobService, 
        private departmentService: DepartmentService,
        private labelService: LabelService,
        private difficultyService: DifficultyService,
        private questionService: QuestionService,
        private formBuilder: FormBuilder,
        private changeDetectionRef: ChangeDetectorRef,
        private router: Router,
        private activatedRoute:ActivatedRoute){
    }

    ngOnInit(){
        this.jobForm = this.formBuilder.group({
            title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
            profile: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(300)]],
            department : ['', Validators.required],
            labelsAndDifficulties: this.formBuilder.array([this.getNewLabelDifficultyGroup()], this.duplicateLabelDiffValidator(this.selectedLabelAndDiffs))
        });

        this.getDepartments();
        this.getLabels();
        this.getDifficulties();
        this.getQuestionsByLabelAndDifficulty();
    }

    closeProgressModal(){
        this.progressModal.hide();
        this.router.navigate(['viewJobs'], { relativeTo: this.activatedRoute});
    }

    
    noQuestionsValidator(c: AbstractControl): {[key: string]: boolean} | null {
        if(c.value != undefined && c.value == "No questions found"){
            return { 'count': true};
        }

        return null;
    }

    validateJobTitle() {
        let jobTitle = this.jobForm.get('title').value;
        this.jobService.IsJobExists(jobTitle).subscribe(
            jobTitleExists => {
                if (jobTitleExists) {
                    this.isEnteredTitleExists = true;
                }
                else
                {
                    this.isEnteredTitleExists = false;
                }
                   
            }
        );
    }

    duplicateLabelDiffValidator(selectedLabelAndDiffs: LabelAndDiff[]): ValidatorFn {
        return (formArray: FormArray): {[key: string]: boolean} | null => {
            let formArrayErrorCount = 0;

            Object.keys(selectedLabelAndDiffs).forEach((first, indexFirst)=> {
                    let i = selectedLabelAndDiffs[indexFirst];
                    Object.keys(selectedLabelAndDiffs).forEach((second, indexSecond)=>{
                        let j = selectedLabelAndDiffs[indexSecond];
                        if(i != j){
                            if(i.labelId == j.labelId && i.diffId == j.diffId){
                                formArrayErrorCount++;
                            }
                        }
                    });
                })

            if(formArrayErrorCount > 0){
                return { 'duplicate': true }
            }
            else{
                return null;
            }
        }
    }

    save(): void{
        this.progressModal.show();
        this.modalResponse = "Creating job";

        this.job.Title = this.jobForm.get('title').value;
        this.job.Profile = this.jobForm.get('profile').value;
        this.job.Department = new DepartmentDto(this.jobForm.get('department').value);
        this.job.JobDifficultyLabels = [];
        let labelsAndDifficulties: Array<any> = this.jobForm.get('labelsAndDifficulties').value;

        labelsAndDifficulties.forEach(item => {
            let jobDifficultyLabel = new Job_Difficulty_LabelDto();
            jobDifficultyLabel.Difficulty = new DifficultyDto(item.difficulty);
            jobDifficultyLabel.Label = new LabelDto(item.label);
            jobDifficultyLabel.DisplayQuestionCount = item.availableQuestions;
            jobDifficultyLabel.PassingQuestionCount = item.questionsToPass;
            this.job.JobDifficultyLabels.push(jobDifficultyLabel);
        });

        this.jobService.Create(this.job).subscribe(
            result => {
                this.isRequestProcessing = false;
                this.jobForm.reset();
                this.modalResponse = "Job created";
            }, 
            error => {
                console.log(error);
                this.isRequestProcessing = false;
                this.modalResponse = "Job creation failed";
            }
        );
    }

    removeLabelAndDifficulty(labelAndDifficultyIndex: number): void{
        this.labelsAndDifficulties.removeAt(labelAndDifficultyIndex);
        this.selectedLabelAndDiffs.splice(labelAndDifficultyIndex, 1);
        this.jobForm.get('labelsAndDifficulties').updateValueAndValidity();

        this.availableQuestionsMap = this.rearrangeQuestionMaps(labelAndDifficultyIndex, this.availableQuestionsMap);
        if(this.questionsToPassMap && labelAndDifficultyIndex < this.questionsToPassMap.length){
            this.questionsToPassMap = this.rearrangeQuestionMaps(labelAndDifficultyIndex, this.questionsToPassMap);
        }
    }

    addLabelAndDifficulty(): void{
        this.labelsAndDifficulties.push(this.getNewLabelDifficultyGroup())
    }

    private rearrangeQuestionMaps(indexToBeRemoved: number, arrayToBeReArranged: Array<any>): Array<any> {
        arrayToBeReArranged.splice(indexToBeRemoved, 1);
        return arrayToBeReArranged;
    }
    
    private getNewLabelDifficultyGroup(): FormGroup {
        let dynamicFormGroup = this.formBuilder.group({
                label: ['', Validators.required],
                difficulty: ['', Validators.required],
                availableQuestions: ['', [Validators.required, this.noQuestionsValidator]],
                questionsToPass: ['', [Validators.required, this.noQuestionsValidator]]
            });

        let availableQuestionControlIndex = 0;

        if(this.jobForm){
            availableQuestionControlIndex = this.labelsAndDifficulties.length;
        }
        
        this.subscribeToValueChanges(dynamicFormGroup, availableQuestionControlIndex);

        return dynamicFormGroup;
    }
    
    private subscribeToValueChanges(dynamicFormGroup: FormGroup, formGroupIndex: number){
        dynamicFormGroup.controls['label'].valueChanges.subscribe(labelValue => {
            let difficultyValue = dynamicFormGroup.controls['difficulty'].value;
            
            if(difficultyValue){
                this.setQuestionCounts(dynamicFormGroup, formGroupIndex, difficultyValue, labelValue);
            }
        });

        dynamicFormGroup.controls['difficulty'].valueChanges.subscribe(difficultyValue => {
            let labelValue = dynamicFormGroup.controls['label'].value;
            
            if(labelValue){
                this.setQuestionCounts(dynamicFormGroup, formGroupIndex, difficultyValue, labelValue);
            }
        });
    }

    private getArray(size: number){
        let arr = [];
        for(let i = 1 ;i <= size; i++){
            arr.push(i);
        }

        return arr;
    }

    onAvailableChange(i: number){
        console.log(i);
        let availValue = this.jobForm.get('labelsAndDifficulties').get(i.toString()).get('availableQuestions').value
        let questionToPassCount: number = Number(availValue);
        this.questionsToPassMap[i] = this.getArray(questionToPassCount);
    }

    private setQuestionCounts(dynamicFormGroup: FormGroup, formGroupIndex: number, difficultyValue: any, labelValue: any) {

        let matchingCombinationOfLabelAndDiff = this.questionDifficultyLabels != null && this.questionDifficultyLabels.find(item => item.DifficultyId == difficultyValue && item.LabelId == labelValue);
        let questionCount = matchingCombinationOfLabelAndDiff ? matchingCombinationOfLabelAndDiff.QuestionCount : 0;

        let availableQuestionsControl = dynamicFormGroup.controls['availableQuestions'];
        let questionsToPassControl = dynamicFormGroup.controls['questionsToPass'];

        let questionNotFoundText = "No questions found";
        if(!matchingCombinationOfLabelAndDiff){
            

            availableQuestionsControl.reset();
            this.availableQuestionsMap[formGroupIndex] = [questionNotFoundText];
            availableQuestionsControl.setValue(questionNotFoundText);
            //availableQuestionsControl.disable();

            questionsToPassControl.reset();
            this.questionsToPassMap[formGroupIndex] = [questionNotFoundText];
            questionsToPassControl.setValue(questionNotFoundText);
            //questionsToPassControl.disable();
            
        }
        else{
            let selectedLAndDObj = this.selectedLabelAndDiffs[formGroupIndex];
            if(selectedLAndDObj){
                selectedLAndDObj.labelId = labelValue;
                selectedLAndDObj.diffId = difficultyValue;
            } 
            else{
                selectedLAndDObj = new LabelAndDiff(labelValue, difficultyValue);
            }
            this.selectedLabelAndDiffs[formGroupIndex] = selectedLAndDObj;

            //console.log(this.selectedLabelAndDiffs[formGroupIndex]);
            // availableQuestionsControl.enable();
            // questionsToPassControl.enable();
            let availableQuestionControlValue = availableQuestionsControl.value
            
            this.availableQuestionsMap[formGroupIndex] = this.getArray(questionCount);
            if(availableQuestionControlValue === questionNotFoundText){
                availableQuestionsControl.setValue('');
                availableQuestionsControl.markAsTouched();

                questionsToPassControl.setValue('');
                questionsToPassControl.markAsTouched();
            }
        }
    }
    private getDepartments(){
        this.departmentService.GetAll().subscribe(
            departments => this.departments = departments,
            error=> console.log(error)
        );
    }

    private getLabels(){
        this.labelService.GetAllLabels().subscribe(
            labels => this.labels = labels,
            error=> console.log(error)
        );
    }

    private getDifficulties(){
        this.difficultyService.GetAllDifficulties().subscribe(
            difficulties => this.difficulties = difficulties,
            error => console.log(error)
        );
    }

    private getQuestionsByLabelAndDifficulty(){
        this.questionService.GetQuestionsByLabelAndDifficulty().subscribe(
            questionDifficultyLabels => this.questionDifficultyLabels = questionDifficultyLabels,
            error => console.log(error)
        );
    }

}

class LabelAndDiff{
    constructor(labelId, diffId){
        this.labelId = labelId;
        this.diffId = diffId;
    }
    labelId: number;
    diffId: number;
}