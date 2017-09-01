import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { SurveyResponseDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SurveyService{
    private surveyResponseApiUrl: string = '/Survey/';
    constructor(private http: Http) { }

    public IsSurveyAssigned(): Observable<any> {
        return this.http.get(`${this.surveyResponseApiUrl}IsSurveyAssigned`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public IsSurveyFinished(): Observable<any> {
        return this.http.get(`${this.surveyResponseApiUrl}IsSurveyFinished`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetSurveyChallenge(): Observable<SurveyResponseDto[]> {
        return this.http.get(`${this.surveyResponseApiUrl}GetSurveyChallenge`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public SaveSurveyResponse(surveyResponses: SurveyResponseDto[]): Observable<any> {
        return this.http.post(`${this.surveyResponseApiUrl}SaveSurveyResponses`, surveyResponses).map(
            (response: Response) => {
                return response.json();
            });
    }
}