import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { Candidate_JobDto, TestResultDto, TestDto } from '../../RemoteServicesProxy';
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

    public GetAllTestResults(): Observable<TestResultDto[]> {
        return this.http.get(`${this.testApiUrl}GetTestResults`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetArchivedTestResults(): Observable<TestResultDto[]> {
        return this.http.get(`${this.testApiUrl}GetArchivedTestResults`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetFinishedTestDetail(testId: number): Observable<TestDto> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this.http.post(`${this.testApiUrl}GetFinishedTestDetail`, testId, options).map(
            (response: Response) => {
                return response.json();
            });
    }

    public ArchiveTests(testIds: number[]): Observable<TestDto> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this.http.post(`${this.testApiUrl}ArchiveTests`, testIds, options).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetExcelFileForAllActiveTests(): Observable<any> {
            return this.http.get(`${this.testApiUrl}ExportAllTests`, { responseType: ResponseContentType.Blob }).map(response => response.blob());
    }

    public GetExcelFileForArchivedTests(): Observable<any> {
        return this.http.get(`${this.testApiUrl}ExportArchivedTests`, { responseType: ResponseContentType.Blob }).map(response => response.blob());
    }

    public GetExcelFileForFinishedTestsByJob(jobId: number): Observable<any> {
            return this.http.get(`${this.testApiUrl}ExportFinishedTestsByJob?jobId=${jobId}`, { responseType: ResponseContentType.Blob }).map(response => response.blob());
    }
}