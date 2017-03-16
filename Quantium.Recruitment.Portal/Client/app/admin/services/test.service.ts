import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { Candidate_JobDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class TestService{
    private testApiUrl: string = '/Test/';

    constructor(private http: Http) { }

    public Generate(candidateJobs: Candidate_JobDto[]): Observable<any> {
        return this.http.post(`${this.testApiUrl}Generate`, candidateJobs).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAll(): Observable<any> {
        return this.http.get(`${this.testApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAllTestResults(): Observable<any> {
        return this.http.get(`${this.testApiUrl}GetTestResults`).map(
            (response: Response) => {
                return response.json();
            });
    }
}