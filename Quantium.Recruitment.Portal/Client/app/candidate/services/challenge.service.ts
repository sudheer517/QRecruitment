import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { ChallengeDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class ChallengeService{
    private challengeApiUrl: string = '/Challenge/';

    constructor(private http: Http) { }

    public GetNextChallenge(): Observable<ChallengeDto> {
        return this.http.get(`${this.challengeApiUrl}Get`).map(
            (response: Response) => {
                //console.log(response.json());
                return response.json();
            });
    }

    public IsTestFinished(): Observable<boolean> {
        return this.http.get(`${this.challengeApiUrl}IsTestFinished`).map(
            (response: Response) => {
                //console.log(response.json());
                return response.json();
            });
    }

    public IsTestAssigned(): Observable<boolean> {
        return this.http.get(`${this.challengeApiUrl}IsTestAssigned`).map(
            (response: Response) => {
                //console.log(response.json());
                return response.json();
            });
    }
    
    public PostChallenge(challenge: ChallengeDto): Observable<any> {
        return this.http.post(`${this.challengeApiUrl}Post`, challenge).map(
            (response: Response) => {
                //console.log(response.json());
                return response.json();
            });
    }

    public FinishAllChallenges(testId: number): Observable<any> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this.http.post(`${this.challengeApiUrl}FinishAllChallenges`, testId, options).map(
            (response: Response) => {
                //console.log(response.json());
                return response.json();
            });
    }
}