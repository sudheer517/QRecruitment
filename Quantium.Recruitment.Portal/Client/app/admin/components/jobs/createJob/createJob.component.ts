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
    availableQuestionsMap: QuestionsInJobMap = new QuestionsInJobMap();
    questionsToPassMap: QuestionsInJobMap = new QuestionsInJobMap();
    @ViewChild('progress') progressModal:ModalDirective;
    isRequestProcessing: boolean = true;
    modalResponse: string;
    isEnteredTitleExists: boolean = false;
    selectedLabelAndDiffs = new SelectedLabelAndDiffs();

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
            labelsAndDifficulties: this.formBuilder.array([this.getNewLabelDifficultyGroup()])
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
        this.jobService.GetAllJobs().subscribe(
            result => {
                if (result.find(job => job.Title == jobTitle))
                    this.isEnteredTitleExists = true;
                else
                    this.isEnteredTitleExists = false;
            }
        );
    }
    duplicateLabelDiffValidator(selectedLabelAndDiffs: SelectedLabelAndDiffs): ValidatorFn {

        return (c: any): {[key: string]: boolean} | null => {
            
            let labelId = c.get('label').value;
            let difficultyId = c.get('difficulty').value;
            let count = 0;
            Object.keys(selectedLabelAndDiffs).forEach((item, index)=> {
                let labelAndDiff = selectedLabelAndDiffs[0];
                if(labelAndDiff.labelId == labelId && labelAndDiff.diffId == difficultyId){
                    count++;
                }
            })

            if(count > 1){
                return { 'duplicate': true }
            }

            return null;
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

        this.availableQuestionsMap = this.rearrangeQuestionMaps(labelAndDifficultyIndex, this.availableQuestionsMap);
        this.questionsToPassMap = this.rearrangeQuestionMaps(labelAndDifficultyIndex, this.questionsToPassMap);
    }

    addLabelAndDifficulty(): void{
        this.labelsAndDifficulties.push(this.getNewLabelDifficultyGroup())
    }

    private rearrangeQuestionMaps(indexToBeRemoved: number, arrayToBeReArranged: QuestionsInJobMap): QuestionsInJobMap {
        let rearrangedMap = new QuestionsInJobMap();

        for(let i = 0 ; i < indexToBeRemoved; i++){
            rearrangedMap[i] = arrayToBeReArranged[i];
        }

        for(let i = indexToBeRemoved ; i < (Object.keys(arrayToBeReArranged).length - 1); i++){
            rearrangedMap[i] = arrayToBeReArranged[i + 1];
        }

        return rearrangedMap;
    }
    
    private getNewLabelDifficultyGroup(): FormGroup {
        let dynamicFormGroup = this.formBuilder.group({
                label: ['', Validators.required],
                difficulty: ['', Validators.required],
                availableQuestions: ['', [Validators.required, this.noQuestionsValidator]],
                questionsToPass: ['', [Validators.required, this.noQuestionsValidator]]
            }, { validator: this.duplicateLabelDiffValidator(this.selectedLabelAndDiffs) });

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

        dynamicFormGroup.controls['availableQuestions'].valueChanges.subscribe(availableQuestionsValue => {
            let questionToPassCount: number = Number(availableQuestionsValue);
            this.questionsToPassMap[formGroupIndex] = this.getArray(availableQuestionsValue);
        });
    }

    private getArray(size: number){
        let arr = [];
        for(let i = 1 ;i <= size; i++){
            arr.push(i);
        }

        return arr;
    }

    private setQuestionCounts(dynamicFormGroup: FormGroup, formGroupIndex: number, difficultyValue: any, labelValue: any ){
        
        let matchingCombinationOfLabelAndDiff = this.questionDifficultyLabels.find(item => item.DifficultyId == difficultyValue && item.LabelId == labelValue);
        let questionCount = matchingCombinationOfLabelAndDiff ? matchingCombinationOfLabelAndDiff.QuestionCount : 0;

        let availableQuestionsControl = dynamicFormGroup.controls['availableQuestions'];
        let questionsToPassControl = dynamicFormGroup.controls['questionsToPass'];

        if(!matchingCombinationOfLabelAndDiff){
            let questionNotFoundText = "No questions found";

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
            availableQuestionsControl.enable();
            questionsToPassControl.enable();
            this.availableQuestionsMap[formGroupIndex] = this.getArray(questionCount);
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

export class QuestionsInJobMap{
    [key: number]: any[];
}

class LabelAndDiff{
    constructor(labelId, diffId){
        this.labelId = labelId;
        this.diffId = diffId;
    }
    labelId: number;
    diffId: number;
}
class SelectedLabelAndDiffs{
    [key: number]:  LabelAndDiff;
}