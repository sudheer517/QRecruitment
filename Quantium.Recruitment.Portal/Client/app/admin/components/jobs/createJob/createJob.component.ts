import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, AbstractControl, FormArray } from '@angular/forms';

import { JobService } from '../../../services/job.service';
import { DepartmentService } from '../../../services/department.service';
import { LabelService } from '../../../services/label.service';
import { DifficultyService } from '../../../services/difficulty.service';
import { QuestionService } from '../../../services/question.service';

import { JobDto, DepartmentDto, LabelDto, DifficultyDto, Question_Difficulty_LabelDto } from '../../../../RemoteServicesProxy';


@Component({
    selector: 'appc-create-job',
    templateUrl: './createJob.component.html',
    styleUrls: ['./createJob.component.scss']
})
export class CreateJobComponent implements OnInit {
    jobForm: FormGroup;
    job: JobDto = new JobDto();
    availableQuestionsMap: AvailableQuestionsMap = new AvailableQuestionsMap();

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
        private formBuilder: FormBuilder){
    }

    ngOnInit(){
        this.jobForm = this.formBuilder.group({
            title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
            profile: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(300)]],
            department : ['', Validators.required],
            labelsAndDifficulties: this.formBuilder.array([])
        });

        this.getDepartments();
        this.getLabels();
        this.getDifficulties();
        this.getQuestionsByLabelAndDifficulty();
    }

    save(): void{
        JSON.stringify(this.jobForm.value);
    }

    removeLabelAndDifficulty(labelAndDifficultyIndex: number): void{
        this.labelsAndDifficulties.removeAt(labelAndDifficultyIndex);
    }

    addLabelAndDifficulty(): void{
        this.labelsAndDifficulties.push(this.getNewLabelDifficultyGroup())
    }
    
    valueChanged(changedFormArrayIndex: number){
        // console.log(changedFormGroup);

        // console.log(changedFormGroup.value);
        // let fc = changedFormGroup.controls["label"] as FormControl;

        // fc.valueChanges.subscribe(value => { console.log('fc value'); console.log(value); })
        // console.log(changedFormGroup.get('availableQuestions').value);
        // console.log(changedFormGroup.get('questionsToPass').value);
        // console.log(changedFormGroup.get('difficulty').value);
        
    }

    private getNewLabelDifficultyGroup(): FormGroup {
        let dynamicFormGroup = this.formBuilder.group({
                label: ['', Validators.required],
                difficulty: ['', Validators.required],
                availableQuestions: ['', Validators.required],
                questionsToPass: ['', Validators.required]
            });

        let availableQuestionControlIndex = this.labelsAndDifficulties.length;
        
        this.subscribeToValueChanges(dynamicFormGroup, availableQuestionControlIndex);

        return dynamicFormGroup;
    }

    private subscribeToValueChanges(dynamicFormGroup: FormGroup, formGroupIndex: number){
        dynamicFormGroup.controls['label'].valueChanges.subscribe(labelValue => {
            let difficultyValue = dynamicFormGroup.controls['difficulty'].value;
            
            if(difficultyValue){
                let questionCount = this.questionDifficultyLabels.find(item => item.DifficultyId == difficultyValue && item.LabelId == labelValue).QuestionCount
                
                this.availableQuestionsMap[formGroupIndex] = new Array(questionCount);
            }
        });

        dynamicFormGroup.controls['difficulty'].valueChanges.subscribe(difficultyValue => {
            let labelValue = dynamicFormGroup.controls['label'].value;
            
            if(labelValue){
                let questionCount = this.questionDifficultyLabels.find(item => item.DifficultyId == difficultyValue && item.LabelId == labelValue).QuestionCount
                
                this.availableQuestionsMap[formGroupIndex] = new Array(questionCount);
            }
        });
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

export class AvailableQuestionsMap{
    [key: number]: number[];
}