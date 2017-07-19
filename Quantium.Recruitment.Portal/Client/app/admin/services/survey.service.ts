import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType, URLSearchParams } from '@angular/http';
import { SurveyQuestionDto, SurveyResponseDto, SurveyAdminCommentsDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SurveyService{
    private surveyApiUrl: string = '/Survey/';

    constructor(private http: Http) { }

      // Survey Questions 
    public AddQuestions(file: any): Observable<Number> {
        return this.http.post(`${this.surveyApiUrl}AddQuestions`, file).map(
            (response: Response) => {
                return response.status;
            });
    }

    public GetAllQuestions(): Observable<SurveyQuestionDto[]> {
        return this.http.get(`${this.surveyApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }
      

    public DeleteQuestion(questionId: number): Observable<Number> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        console.log(questionId);
        return this.http.post(`${this.surveyApiUrl}MarkQuestionInActive`, questionId, options).map(
            (response: Response) => {
                return response.status;
            });
    }

    // Survey Responses 
    public GetSurveydCandidates(): Observable<any> {
        return this.http.get(`${this.surveyApiUrl}GetSurveydCandidates`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetSurveyResponses(candidateId: number): Observable<any> {
        return this.http.get(`${this.surveyApiUrl}GetSurveyResponses?candidateId=${candidateId}`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public CreateSurvey(candidateIds: number[]): Observable<SurveyResponseDto> {
        return this.http.post(`${this.surveyApiUrl}Create`, candidateIds).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetSurveyAdminComments(responseIds: number[]): Observable<any> {
        return this.http.post(`${this.surveyApiUrl}GetSurveyAdminComments`, responseIds).map(
            (response: Response) => {
                return response.json();
            });
    }

    public AddSurveyAdminComments(adminComments: SurveyAdminCommentsDto): Observable<any> {
        return this.http.post(`${this.surveyApiUrl}AddSurveyAdminComments`, adminComments).map(
            (response: Response) => {
                return response.json();
            });
    }
}