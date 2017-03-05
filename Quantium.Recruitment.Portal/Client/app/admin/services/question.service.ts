import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestOptionsArgs, ResponseContentType } from '@angular/http';
import { QuestionDto, Question_Difficulty_LabelDto } from '../../RemoteServicesProxy';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class QuestionService{
    private questionApiUrl: string = 'Question/';

    constructor(private http: Http) { }

    public PreviewQuestions(file: any): Observable<QuestionDto[]> {
        return this.http.post(`${this.questionApiUrl}PreviewQuestions`, file).map(
            (response: Response) => {
                return response.json();
            });
    }

    public AddQuestions(file: any): Observable<Number> {
        return this.http.post(`${this.questionApiUrl}AddQuestions`, file).map(
            (response: Response) => {
                return response.status;
            });
    }

    public GetAllQuestions(): Observable<QuestionDto[]> {
        return this.http.get(`${this.questionApiUrl}GetAll`).map(
            (response: Response) => {
                return response.json();
            });
    }

    public GetQuestionsByLabelAndDifficulty(): Observable<Question_Difficulty_LabelDto[]> {
        return this.http.get(`${this.questionApiUrl}GetQuestionsByLabelAndDifficulty`).map(
            (response: Response) => {
                return response.json();
            });
    }
}