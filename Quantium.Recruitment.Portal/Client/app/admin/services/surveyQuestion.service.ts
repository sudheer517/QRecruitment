import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { SurveyQuestionDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SurveyQuestionService {
    private surveyQuestionApiUrl: string = '/SurveyQuestion/';

    constructor(private http: Http) { }

    public PreviewSurveyQuestions(file: any): Observable<SurveyQuestionDto[]> {
        return this.http.post(`${this.surveyQuestionApiUrl}PreviewSurveyQuestions`, file).map(
            (response: Response) => {
                return response.json();
            });
    }

    public AddSurveyQuestions(file: any): Observable<Number> {
        return this.http.post(`${this.surveyQuestionApiUrl}AddSurveyQuestions`, file).map(
            (response: Response) => {
                return response.status;
            });
    }

    public GetAllSurveyQuestions(): Observable<SurveyQuestionDto[]> {
        return this.http.get(`${this.surveyQuestionApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }
}