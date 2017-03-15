import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { CandidateDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class SurveyService {
    private surveyApiUrl: string = '/Survey/';

    constructor(private http: Http) { }

    public Generate(candidates: CandidateDto[]): Observable<CandidateDto[]> {
        return this.http.post(`${this.surveyApiUrl}Generate`, candidates).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAll(): Observable<any> {
        return this.http.get(`${this.surveyApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }
}