import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Router, ActivatedRoute } from '@angular/router';
import { SurveyService } from '../../../services/survey.service';



@Component({
    selector: 'appc-add-survey-questions',
    templateUrl: './add-survey-questions.component.html',
    styleUrls: ['./add-survey-questions.component.scss']
})
export class AddSurveyQuestionsComponent implements OnInit {
    fileText = "Choose file";
    fileModel;
    fileData: any;
    @ViewChild('progress') progressModal: ModalDirective;
    modalResponse: string;
    questionsSaved: boolean;
    constructor(private questionService: SurveyService, private router: Router, private activatedRoute: ActivatedRoute) { }

    ngOnInit() {
        let body = document.getElementsByTagName('input')[0];
    }
    browseClick(fileInput: any) {
        let fileInputControl: FormControl = fileInput.control as FormControl;
        fileInputControl.reset();
        this.fileText = "Choose file";
    }
    onFileChange(eventData: any) {
        this.fileText = eventData.target.value.split("\\").pop();
        this.fileData = eventData;
        let formData = this.getFileFormData(this.fileData);
    }
    private getFileFormData(eventData: any) {
        let formData: FormData = new FormData();

        formData.append('uploadFile', eventData.target.files[0], eventData.target.files[0].name);
        return formData;
    }
    addQuestions(modalContent: FormControl) {
        this.progressModal.show();
        this.modalResponse = "Uploading questions";
        let formData = this.getFileFormData(this.fileData);
        this.questionService.AddQuestions(formData).subscribe(
            status => {
                this.modalResponse = "Questions uploaded";
                this.questionsSaved = true;
            },
            (error) => {
                if (error.status == 406) {

                    this.modalResponse = "Questions data validation failed. Please upload correct data";

                    this.progressModal.show();
                }
                if (error.status == 409) {

                    this.modalResponse = "Duplicate question found for Id:" + error.json();

                    this.progressModal.show();
                }
                else {

                    this.modalResponse = "Questions upload failed";
                }
            }
        );
    }

    closeProgressModal() {
        this.progressModal.hide();
        if (this.questionsSaved) {
            this.router.navigate(['viewQuestions'], { relativeTo: this.activatedRoute });
        }


    }
}
