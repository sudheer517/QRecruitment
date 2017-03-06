import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { JobDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class JobService{
    private jobApiUrl: string = 'Job/';

    constructor(private http: Http) { }

    // public PreviewQuestions(file: any): Observable<QuestionDto[]> {
    //     return this.http.post(`${this.questionApiUrl}PreviewQuestions`, file).map(
    //         (response: Response) => {
    //             return response.json();
    //         });
    // }

    // public AddQuestions(file: any): Observable<Number> {
    //     return this.http.post(`${this.questionApiUrl}AddQuestions`, file).map(
    //         (response: Response) => {
    //             return response.status;
    //         });
    // }

    public Create(job: JobDto): Observable<any> {
        return this.http.post(`${this.jobApiUrl}Create`, job).map(
            (response: Response) => {
                return response.json();
            });
    }
}