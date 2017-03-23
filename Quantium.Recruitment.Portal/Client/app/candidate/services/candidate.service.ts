import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { CandidateDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class CandidateService{
    private candidateApiUrl: string = '/Candidate/';
    constructor(private http: Http) { }

    public Get(): Observable<CandidateDto> {
        return this.http.get(`${this.candidateApiUrl}Get`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public IsInformationFilled(): Observable<boolean> {
        return this.http.get(`${this.candidateApiUrl}IsInformationFilled`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public SaveDetails(candidate: CandidateDto): Observable<any> {
        return this.http.post(`${this.candidateApiUrl}SaveDetails`, candidate).map(
            (response: Response) => {
                return response.json();
            });
    }
}