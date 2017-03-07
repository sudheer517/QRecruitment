import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { JobDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class JobService{
    private jobApiUrl: string = 'Job/';

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

    public DeleteJob(job: JobDto): Observable<any> {
        return this.http.post(`${this.jobApiUrl}Delete`, job).map(
            (response: Response) => {
                return response.json();
            });
    }
}