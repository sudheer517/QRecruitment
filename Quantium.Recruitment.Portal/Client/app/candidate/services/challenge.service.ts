import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { Candidate_JobDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class ChallengeService{
    private testApiUrl: string = '/Challenge/';

    constructor(private http: Http) { }

    public GetNextChallenge(): Observable<any> {
        return this.http.get(`${this.testApiUrl}Generate`).map(
            (response: Response) => {
                return response.json();
            });
    }
}