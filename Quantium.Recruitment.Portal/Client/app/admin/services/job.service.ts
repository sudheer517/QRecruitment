import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { JobDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class JobService{
    private jobApiUrl: string = '/Job/';

    constructor(private http: Http) { }


    public Create(job: JobDto): Observable<any> {
        return this.http.post(`${this.jobApiUrl}Create`, job).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetAllJobs(): Observable<JobDto[]> {
        return this.http.get(`${this.jobApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public DeleteJob(jobId: number): Observable<any> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers });

        return this.http.post(`${this.jobApiUrl}Delete`, jobId, options).map(
            (response: Response) => {
                return response.json();
            });
    }

    public IsJobExists(jobTitle: string): Observable<boolean> {
        return this.http.get(`${this.jobApiUrl}IsJobExists?title=`+ jobTitle).map(
            (response: Response) => {
                return response.json();
            });
    }
}