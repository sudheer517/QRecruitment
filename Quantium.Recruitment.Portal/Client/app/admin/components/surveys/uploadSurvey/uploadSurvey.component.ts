import { Component, OnInit, ViewChild } from '@angular/core';
import { Renderer, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Observable } from 'rxjs/Observable';
import { FormControl } from '@angular/forms';
import { SurveyQuestionDto } from '../../../../RemoteServicesProxy';
import { Router, ActivatedRoute } from '@angular/router';
import { CandidateService } from '../../../services/candidate.service';
import { SurveyQuestionService } from '../../../services/surveyQuestion.service';


@Component({
    selector: 'appc-upload-survey',
    templateUrl: './uploadSurvey.component.html'
})
export class UploadSurveyComponent implements OnInit {
    @ViewChild('progress') progressModal: ModalDirective;
    smallModalStatus = false;
    surveyQuestions: SurveyQuestionDto[];
    fileData: any;
    fileText = "Choose file";
    constructor(private renderer: Renderer, private surveyQuestionService: SurveyQuestionService, private router: Router, private activatedRoute: ActivatedRoute) {

    }
    ngOnInit() {
        let body = document.getElementsByTagName('input')[0];        
    }
    addQuestions(modalContent: FormControl) {
        this.progressModal.show();
        
        let formData = this.getFileFormData(this.fileData);
        this.surveyQuestionService.AddSurveyQuestions(formData).subscribe(
            status => {
                console.log(status);
                },
            error => {
                console.log(error);
                this.smallModalStatus = true;
            }
        );
    }

    private getFileFormData(eventData: any) {
        let formData: FormData = new FormData();

        formData.append('uploadFile', eventData.target.files[0], eventData.target.files[0].name);
        return formData;
    }
}