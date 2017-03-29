import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { ChallengeDto, FeedbackTypeDto, FeedbackDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class FeedbackService{
    private feedbackApiUrl: string = '/Feedback/';

    constructor(private http: Http) { }

    public GetAllFeedbackTypes(): Observable<FeedbackTypeDto[]> {
        return this.http.get(`/FeedbackType/GetAll`).map(
            (response: Response) => {
                console.log(response.json());
                return response.json();
            });
    }

    public CreateFeedback(feedbackDto: FeedbackDto): Observable<any> {
        return this.http.post(`${this.feedbackApiUrl}Create`, feedbackDto).map(
            (response: Response) => {
                console.log(response.json());
                return response.json();
            });
    }
}