import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { SurveyResponseDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SurveyService{
    private surveyResponseApiUrl: string = '/SurveyResponse/';
    constructor(private http: Http) { }

    public GetSurvey(): Observable<SurveyResponseDto> {
        return this.http.get(`${this.surveyResponseApiUrl}Get`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public CreateSurvey(): Observable<SurveyResponseDto> {
        return this.http.get(`${this.surveyResponseApiUrl}Create`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public SaveSurveyResponse(surveyResponse: SurveyResponseDto): Observable<any> {
        return this.http.post(`${this.surveyResponseApiUrl}SaveDetails`, surveyResponse).map(
            (response: Response) => {
                return response.json();
            });
    }
}