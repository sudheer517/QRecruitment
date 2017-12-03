import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { CandidateDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CandidateService{
    private candidateApiUrl: string = '/Candidate/';

    constructor(private http: Http) { }

    public AddCandidate(candidate: CandidateDto): Observable<CandidateDto> {
        return this.http.post(`${this.candidateApiUrl}AddCandidateAsync`, candidate).map(
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
        return this.http.post(`${this.candidateApiUrl}AddCandidatesAsync`, file).map(
            (response: Response) => {
                return response.status;
            });
    }

    public GetAllCandidates(): Observable<any> {
        return this.http.get(`${this.candidateApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAllCandidatesWithPassword(): Observable<any> {
        return this.http.get(`${this.candidateApiUrl}GetAllWithPassword`).map(
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

    public ArchiveCandidates(candidateIds: number[]): Observable<any> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this.http.post(`${this.candidateApiUrl}ArchiveCandidates`, candidateIds, options).map(
            (response: Response) => {
                return response.json();
            });
    }
}