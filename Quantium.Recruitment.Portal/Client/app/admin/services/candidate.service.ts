import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { CandidateDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CandidateService{
    private candidateApiUrl: string = '/Candidate/';

    constructor(private http: Http) { }

    public AddCandidate(candidate: CandidateDto): Observable<CandidateDto> {
        return this.http.post(`${this.candidateApiUrl}AddCandidate`, candidate).map(
            (response: Response) => {
                return response.json();
            });
    }

    public PreviewCandidates(file: any): Observable<CandidateDto[]> {
        return this.http.post(`${this.candidateApiUrl}PreviewCandidates`, file).map(
            (response: Response) => {
                return response.json();
            });
    }

    public AddCandidates(file: any): Observable<Number> {
        return this.http.post(`${this.candidateApiUrl}AddCandidates`, file).map(
            (response: Response) => {
                return response.status;
            });
    }

    public GetAllCandidates(): Observable<CandidateDto[]> {
        return this.http.get(`${this.candidateApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetCandidateWithoutActiveTests(): Observable<CandidateDto[]>{
        return this.http.get(`${this.candidateApiUrl}GetCandidatesWithoutActiveTests`).map(
            (response: Response) => {
                return response.json();
            });
    }
}