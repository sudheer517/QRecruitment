import { Component, OnInit, ViewChild } from '@angular/core';
import { QuestionService } from '../../../services/question.service';
import { LabelService } from '../../../services/label.service';
import { DifficultyService } from '../../../services/difficulty.service';
import { QuestionDto, PagedQuestionDto, LabelDto, DifficultyDto } from '../../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'appc-view-questions',
    templateUrl: './viewQuestions.component.html',
    styleUrls: ['./viewQuestions.component.scss']
})
export class ViewQuestionsComponent implements OnInit{
    questions: QuestionDto[];
    labels: LabelDto[];
    difficulties: DifficultyDto[];
    selectedLabel: number;
    selectedDifficulty: number;

    isRequestProcessing = true;
    modalResponse: string;
    @ViewChild('progress') progressModal: ModalDirective;

    public page:number = 1;
    public length: number = 0;
    public itemsPerPage:number = 10;
    public maxSize:number = 5;
    public numPages:number = 1;

    constructor(private questionService: QuestionService, private labelService: LabelService, private difficultyService: DifficultyService){

    }

    ngOnInit(){
        this.questionService.GetAllQuestionsByPaging(1, this.itemsPerPage, 0, 0).subscribe(
            pagedQuestionDto => {
                this.questions = pagedQuestionDto.questions;
                this.length = pagedQuestionDto.totalQuestions;
                this.numPages = pagedQuestionDto.totalPages;
            },
            error => console.log(error)
        )

        this.labelService.GetAllLabels().subscribe(
            labels => this.labels = labels
        );

        this.difficultyService.GetAllDifficulties().subscribe(
            difficulties => this.difficulties = difficulties
        );
    }

    labelOrDifficultyChanged(){
        this.getQuestions(1);
    }

    getQuestions(pageNum: number){
        this.questionService.GetAllQuestionsByPaging(pageNum, this.itemsPerPage, this.selectedLabel, this.selectedDifficulty).subscribe(
            pagedQuestionDto => {
                if(pagedQuestionDto.questions && pagedQuestionDto.questions.length > 0){
                    this.questions = pagedQuestionDto.questions;
                    this.length = pagedQuestionDto.totalQuestions;
                    this.numPages = pagedQuestionDto.totalPages;
                }
                else{
                    this.modalResponse = "No questions found";
                    this.isRequestProcessing = false;
                    this.progressModal.show();
                }
            },
            error => console.log(error)
        )
    }

    changePage(event: any){
        this.getQuestions(event.page);
    }
    
    closeModal(){
        this.progressModal.hide();
    }

    deleteQuestion(questionId: number){
        this.modalResponse = "Deleting question";
        this.progressModal.show();
        console.log(questionId);
        this.questionService.DeleteQuestion(questionId).subscribe(
            deleted => {
                this.isRequestProcessing = false;
                this.modalResponse = "Question deleted";
                this.questions = this.questions.filter(q => q.Id !== questionId);
            },
            error => {
                this.isRequestProcessing = false;
                this.modalResponse = "Unable to delete question";
                console.log("unable to delete question");
            }
        )
    }
}