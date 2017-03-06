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
    availableQuestionsMap: QuestionsInJobMap = new QuestionsInJobMap();
    questionsToPassMap: QuestionsInJobMap = new QuestionsInJobMap();

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
            labelsAndDifficulties: this.formBuilder.array([this.getNewLabelDifficultyGroup()])
        });

        this.getDepartments();
        this.getLabels();
        this.getDifficulties();
        this.getQuestionsByLabelAndDifficulty();
    }

    save(): void{
        //console.log(JSON.stringify(this.jobForm.value));
    }

    removeLabelAndDifficulty(labelAndDifficultyIndex: number): void{
        this.labelsAndDifficulties.removeAt(labelAndDifficultyIndex);
    }

    addLabelAndDifficulty(): void{
        this.labelsAndDifficulties.push(this.getNewLabelDifficultyGroup())
    }
    
    private getNewLabelDifficultyGroup(): FormGroup {
        let dynamicFormGroup = this.formBuilder.group({
                label: ['', Validators.required],
                difficulty: ['', Validators.required],
                availableQuestions: ['', Validators.required],
                questionsToPass: ['', Validators.required]
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
            availableQuestionsControl.reset();
            this.availableQuestionsMap[formGroupIndex] = ["No questions found"];
            availableQuestionsControl.setValue("No questions found");
            availableQuestionsControl.disable({onlySelf: true});

            questionsToPassControl.reset();
            this.questionsToPassMap[formGroupIndex] = ["No questions found"];
            questionsToPassControl.setValue("No questions found");
            questionsToPassControl.disable({onlySelf: true});
            
        }
        else{
            availableQuestionsControl.enable({onlySelf: true});
            questionsToPassControl.enable({onlySelf: true});
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